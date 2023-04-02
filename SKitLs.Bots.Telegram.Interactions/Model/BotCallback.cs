using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Interactions.Model
{
    public class BotCallback : BotInteraction, IBotCallback
    {
        public BotCallbackAction Action { get; set; }
        public string Label { get; set; }

        public BotCallback(string @base, string label, BotCallbackAction action) : base(@base)
        {
            Action = action;
            Label = label;
        }

        public BotCallback ExtendLabel(string label)
        {
            Label += " " + label;
            return this;
        }
    }
}