using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.Integration
{
    public interface ITgActorList
    {
        public BotManager Owner { get; }
        public List<IBotInteraction> GetInteractions();
    }
}