using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Integration
{
    public interface ITgActorList<TUpdate> where TUpdate : ICastedUpdate
    {
        public BotManager Owner { set; }
        public List<IBotAction<TUpdate>> GetActionsList();
    }
}