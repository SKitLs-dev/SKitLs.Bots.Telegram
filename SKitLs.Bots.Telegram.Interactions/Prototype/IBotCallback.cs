using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Interactions.Prototype
{
    public delegate Task BotCallbackAction(IBotCallback trigger, SignedCallbackUpdate update);

    public interface IBotCallback : IBotInteraction
    {
        //
        public BotCallbackAction Action { get; }
        public string Label { get; }
    }
}
