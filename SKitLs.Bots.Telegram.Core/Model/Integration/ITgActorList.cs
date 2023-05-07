using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Integration
{
    public interface ITgActorList<TUpdate> where TUpdate : ICastedUpdate
    {
        public BotManager Owner { get; }
        public List<IBotAction<TUpdate>> GetActionsList();
    }
}