using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Management.Implementions
{
    public interface ITgActorList
    {
        public List<IBotInteraction> GetInteractions();
    }
}