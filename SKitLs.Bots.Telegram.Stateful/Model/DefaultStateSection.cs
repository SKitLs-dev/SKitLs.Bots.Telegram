using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Collections;
using System.Collections.ObjectModel;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    /// <summary>
    /// The default implementation of <see cref="IStateSection{TUpdate}"/>.
    /// Provides mechanisms for declaring specific state sections, which are used to fragment handling logics into parts,
    /// based on user's states.
    /// </summary>
    /// <typeparam name="TUpdate">The type of update that this section works with.</typeparam>
    public sealed class DefaultStateSection<TUpdate> : IStateSection<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <inheritdoc/>
        public string? DebugName { get; set; }

        /// <summary>
        /// Determines states that collected actions should react on. Set to <see langword="null"/> to enable any states.
        /// </summary>
        public List<IUserState>? EnabledStates { get; private set; }

        /// <inheritdoc/>
        public bool EnabledAny => EnabledStates is null;

        private List<IBotAction<TUpdate>> SavedActions { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultStateSection{TUpdate}"/> class with the specified data.
        /// </summary>
        /// <param name="name">Optional debug name.</param>
        public DefaultStateSection(string? name = null) => DebugName = name;

        /// <inheritdoc/>
        public List<IUserState> GetEnabledStates() => EnabledStates ?? new();

        /// <inheritdoc/>
        public void EnableState(IUserState state)
        {
            EnabledStates ??= new List<IUserState>();
            EnabledStates.Add(state);
        }

        /// <inheritdoc/>
        public List<IBotAction<TUpdate>> GetActionsList() => SavedActions;

        /// <inheritdoc/>
        public List<IBotAction> GetHeldActions() => GetActionsList().Cast<IBotAction>().ToList();

        /// <inheritdoc/>
        public void AddSafely(IBotAction<TUpdate> action) => SavedActions.Add(!SavedActions.Contains(action)
            ? action
            : throw new DuplicationException(GetType(), typeof(IBotAction<TUpdate>), action.ActionId));

        /// <inheritdoc/>
        public void AddRangeSafely(IEnumerable<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(a => AddSafely(a));

        /// <inheritdoc/>
        public void MergeSafely(IStateSection<TUpdate> section) => AddRangeSafely(section.GetActionsList());

        /// <inheritdoc/>
        public bool Equals(IStateSection<TUpdate>? other)
        {
            if (other is null) return false;
            if (other.EnabledAny || EnabledAny) return other.EnabledAny && EnabledAny;
            return !EnabledStates!.Except(other.GetEnabledStates()).Any();
        }

        /// <inheritdoc/>
        public IEnumerator<IBotAction<TUpdate>> GetEnumerator()
        {
            foreach (var item in GetActionsList())
                yield return item;
        }
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public override string? ToString() => $"{(DebugName is null ? string.Empty : $"({DebugName})")} " +
            $"{(EnabledAny ? "Any states" : (EnabledStates!.Count == 1 ? $"{EnabledStates[0].StateId}" : $"{EnabledStates.Count} states"))} " +
            $"[{SavedActions.Count}]";
    }
}