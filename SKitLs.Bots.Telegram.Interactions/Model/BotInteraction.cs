using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Interactions.Model
{
    public abstract class BotInteraction : IBotInteraction
    {
        public string Base { get; set; }

        public BotInteraction(string @base)
        {
            Base = @base;
        }

        public BotInteraction ExtendBase(string data)
        {
            Base += data;
            return this;
        }

        public bool ShouldBeExecutedOn(string message) => message.ToLower() == Base.ToLower();
        public bool IsSimilarWith(IBotInteraction interaction) => interaction.Base == Base;
    }
}