namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype
{
    [AttributeUsage(AttributeTargets.Class), Obsolete("Will be removed in future versions", true)]
    public class BotArgumentClassAttribute : Attribute
    {
        public bool IsContainer { get; set; }

        public BotArgumentClassAttribute(bool isContainer)
        {
            IsContainer = isContainer;
        }
    }
}