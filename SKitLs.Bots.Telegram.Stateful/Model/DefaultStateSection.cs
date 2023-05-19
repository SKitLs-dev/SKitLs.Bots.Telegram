using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Collections;
using System.Collections.ObjectModel;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    public sealed class DefaultStateSection<TUpdate> : IStateSection<TUpdate> where TUpdate : ICastedUpdate
    {
        public string? DebugName { get; set; }

        public ICollection<IUserState>? EnabledStates { get; private set; }
        public bool EnabledAny => ((IStateSection<TUpdate>)this).EnabledAny;
        private ICollection<IBotAction<TUpdate>> SavedActions { get; set; }

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

        public ICollection<IBotAction<TUpdate>> GetActionsList() => SavedActions;
        // TODO
        public void AddSafely(IBotAction<TUpdate> action) => SavedActions.Add(!SavedActions.Contains(action) ? action : throw new SKTgException(true, "Contains same action!"));
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(a => AddSafely(a));
        public void Apply(IIntegratable<TUpdate> integration) => AddRangeSafely(integration.GetActionsList());

        public bool Equals(IStateSection<TUpdate>? other)
        {
            if (other is null) return false;
            if (other.EnabledAny || EnabledAny) return other.EnabledAny && EnabledAny;
            return EnabledStates!.Except(other.EnabledStates!).Any();
        }
        public IEnumerator<IBotAction<TUpdate>> GetEnumerator()
        {
            foreach (var item in SavedActions)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override string? ToString() => DebugName ?? base.ToString();
    }
}