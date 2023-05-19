using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    public interface IStateSection<TUpdate> : IDebugNamed, IIntegratable<TUpdate>, IEquatable<IStateSection<TUpdate>>, IEnumerable<IBotAction<TUpdate>> where TUpdate : ICastedUpdate
    {
        public ICollection<IUserState>? EnabledStates { get; }
        public bool EnabledAny => EnabledStates is null;
        public void EnableState(IUserState state);
        public bool IsEnabledWith(IUserState state) => EnabledAny || EnabledStates!.ToList().Contains(state);

        public void AddSafely(IBotAction<TUpdate> action);
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions);
        public void Apply(IIntegratable<TUpdate> integration);
    }
}