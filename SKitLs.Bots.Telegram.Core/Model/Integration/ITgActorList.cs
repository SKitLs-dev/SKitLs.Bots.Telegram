using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Integration
{
    public interface ITgActorList
    {
        public BotManager Owner { get; }
        public List<IBotAction<TUpdate>> GetActionsList<TUpdate>() where TUpdate : ICastedUpdate;
    }
}