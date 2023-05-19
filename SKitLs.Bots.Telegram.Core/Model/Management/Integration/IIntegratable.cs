using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Integration
{
    public interface IIntegratable<TUpdate> where TUpdate : ICastedUpdate
    {
        public ICollection<IBotAction<TUpdate>> GetActionsList();
    }
}