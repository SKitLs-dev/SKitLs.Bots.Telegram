using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Interactions.Model
{
    public class BotCommand : BotInteraction, IBotCommand
    {
        public BotCommandAction Action { get; set; }

        public BotCommand(string @base, BotCommandAction action) : base(@base)
        {
            Action = action;
        }
    }
}
