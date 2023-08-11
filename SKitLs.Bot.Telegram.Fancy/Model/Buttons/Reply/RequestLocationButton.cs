using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Reply
{
    /// <summary>
    /// <b>Private chats only.</b> Represents a button that requests the
    /// <see href="https://core.telegram.org/bots/api#keyboardbutton">user's current location</see> when pressed.
    /// </summary>
    public class RequestLocationButton : ReplyButton
    {
        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of the <see cref="RequestLocationButton"/> class with the specified label and optional settings.
        /// </summary>
        public RequestLocationButton(string label, bool singleLine = false) : base(label, singleLine) { }

        /// <inheritdoc/>
        public override KeyboardButton GetButton() => new(Label)
        {
            RequestLocation = true
        };
    }
}