using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Interactions.Prototype
{
    public delegate Task BotCommandAction(IBotCommand trigger, SignedMessageTextUpdate update);

    public interface IBotCommand : IBotInteraction
    {
        public BotCommandAction Action { get; set; }
    }
}
