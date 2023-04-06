namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Attributing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BotActionArgumentAttribute : Attribute
    {
        public uint ArgIndex { get; set; }

        public BotActionArgumentAttribute(uint argIndex)
        {
            ArgIndex = argIndex;
        }
    }
}