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
        public string? DebugName { get; set; }

        public IList<IUserState>? EnabledStates { get; private set; }
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

        public void EnableState(IUserState state)
        {
            EnabledStates ??= new Collection<IUserState>();
            EnabledStates.Add(state);
        }

        public IList<IBotAction<TUpdate>> GetActionsList() => SavedActions;
        public List<IBotAction> GetActionsContent() => GetActionsList().Cast<IBotAction>().ToList();
        public void AddSafely(IBotAction<TUpdate> action) => SavedActions.Add(!SavedActions.Contains(action)
            ? action
            : throw new DuplicationException(GetType(), typeof(IBotAction<TUpdate>), action.ActionId));
        public void AddRangeSafely(IList<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(a => AddSafely(a));
        public void Apply(IIntegratable<TUpdate> integration) => AddRangeSafely(integration.GetActionsList());

        public bool Equals(IStateSection<TUpdate>? other)
        {
            if (other is null) return false;
            if (other.EnabledAny || EnabledAny) return other.EnabledAny && EnabledAny;
            return !EnabledStates!.Except(other.EnabledStates!).Any();
        }
        public IEnumerator<IBotAction<TUpdate>> GetEnumerator()
        {
            foreach (var item in SavedActions)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override string? ToString() => $"{(DebugName is null ? string.Empty : $"({DebugName})")} " +
            $"{(EnabledAny ? "Any states" : (EnabledStates!.Count == 1 ? $"{EnabledStates[0].StateId}" : $"{EnabledStates.Count} states"))} " +
            $"[{SavedActions.Count}]";
    }
}