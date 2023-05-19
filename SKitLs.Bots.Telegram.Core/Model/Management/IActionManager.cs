using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.Management
{
    public interface IActionManager<TUpdate> : IDebugNamed, IOwnerCompilable where TUpdate : ICastedUpdate
    {
        public void AddSafely(IBotAction<TUpdate> action);
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> action);
        public Task ManageUpdateAsync(TUpdate update);
    }
}