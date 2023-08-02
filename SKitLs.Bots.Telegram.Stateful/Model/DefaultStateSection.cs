using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Collections;
using System.Collections.ObjectModel;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    /// <summary>
    /// Default realization of <see cref="IStateSection{TUpdate}"/>.
    /// Provides mechanics of declaring specific state sections, which are used to fragment handling logics into parts,
    /// based on user's states.
    /// </summary>
    /// <typeparam name="TUpdate">Type of update that this section works with.</typeparam>
    public sealed class DefaultStateSection<TUpdate> : IStateSection<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Name, used for simplifying debugging process.
        /// </summary>
        public string? DebugName { get; set; }

        /// <summary>
        /// Determines states that collected actions should react on.
        /// </summary>
        public IList<IUserState>? EnabledStates { get; private set; }
        /// <summary>
        /// Determines, whether collected action should be executed regardless user state.
        /// </summary>
        public bool EnabledAny => EnabledStates is null;
        private IList<IBotAction<TUpdate>> SavedActions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DefaultStateSection{TUpdate}"/> with specified data.
        /// </summary>
        /// <param name="name">Optional. Debug name.</param>
        public DefaultStateSection(string? name = null)
        {
            DebugName = name;
            SavedActions = new Collection<IBotAction<TUpdate>>();
        }

        /// <summary>
        /// Enables custom state.
        /// </summary>
        /// <param name="state">State to enable.</param>
        public void EnableState(IUserState state)
        {
            EnabledStates ??= new Collection<IUserState>();
            EnabledStates.Add(state);
        }

        /// <summary>
        /// Collects all <see cref="IBotAction{TUpdate}"/> declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public IList<IBotAction<TUpdate>> GetActionsList() => SavedActions;
        /// <summary>
        /// Collects all <see cref="IBotAction"/>s declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public List<IBotAction> GetActionsContent() => GetActionsList().Cast<IBotAction>().ToList();

        /// <summary>
        /// Safely adds <paramref name="action"/> to internal storage.
        /// </summary>
        /// <param name="action">Action to be added.</param>
        public void AddSafely(IBotAction<TUpdate> action) => SavedActions.Add(!SavedActions.Contains(action)
            ? action
            : throw new DuplicationException(GetType(), typeof(IBotAction<TUpdate>), action.ActionId));

        /// <summary>
        /// Safely adds range of <paramref name="actions"/> to internal storage.
        /// </summary>
        /// <param name="actions">Actions to be added.</param>
        public void AddRangeSafely(IList<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(a => AddSafely(a));

        /// <summary>
        /// Safely adds data of a new <paramref name="section"/> to internal storage.
        /// </summary>
        /// <param name="section">Section to be merged.</param>
        public void MergeSafely(IStateSection<TUpdate> section) => AddRangeSafely(section.GetActionsList());

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other"></param>
        /// <returns><see langword="true"/> if the current object is equal to the <paramref name="other"/>;
        /// otherwise <see langword="false"/></returns>
        public bool Equals(IStateSection<TUpdate>? other)
        {
            if (other is null) return false;
            if (other.EnabledAny || EnabledAny) return other.EnabledAny && EnabledAny;
            return !EnabledStates!.Except(other.EnabledStates!).Any();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<IBotAction<TUpdate>> GetEnumerator()
        {
            foreach (var item in SavedActions)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override string? ToString() => $"{(DebugName is null ? string.Empty : $"({DebugName})")} " +
            $"{(EnabledAny ? "Any states" : (EnabledStates!.Count == 1 ? $"{EnabledStates[0].StateId}" : $"{EnabledStates.Count} states"))} " +
            $"[{SavedActions.Count}]";
    }
}