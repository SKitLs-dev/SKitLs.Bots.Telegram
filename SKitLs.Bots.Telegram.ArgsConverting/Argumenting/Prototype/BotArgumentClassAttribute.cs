namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BotArgumentClassAttribute : Attribute
    {
        public bool IsContainer { get; set; }

        public BotArgumentClassAttribute(bool isContainer)
        {
            IsContainer = isContainer;
        }
    }
}