namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype
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