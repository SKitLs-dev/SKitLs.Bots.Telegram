using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management
{
    public interface IActionManager<TAction, TUpdate> : IOwnerCompilable where TAction : IBotAction<TUpdate> where TUpdate : CastedUpdate
    {
        public List<TAction> Actions { get; }
        public Task ManageUpdateAsync(TUpdate update);
        public void Apply(ITgActorList<TUpdate> actions);
    }
}