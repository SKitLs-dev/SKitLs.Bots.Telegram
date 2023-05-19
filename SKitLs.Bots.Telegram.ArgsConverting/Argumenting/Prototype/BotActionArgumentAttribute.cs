namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BotActionArgumentAttribute : Attribute
    {
        public int ArgIndex { get; set; }

        public BotActionArgumentAttribute(int argIndex)
        {
            ArgIndex = argIndex;
        }
    }
}