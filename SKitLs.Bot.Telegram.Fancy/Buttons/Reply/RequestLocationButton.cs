using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Reply
{
    /// <summary>
    /// <b>Private chats only.</b> Represents a button that requests the
    /// <see href="https://core.telegram.org/bots/api#keyboardbutton">user's current location</see> when pressed.
    /// </summary>
    public class RequestLocationButton : ReplyButton
    {
        /// <remarks>
        /// Initializes a new instance of the <see cref="RequestLocationButton"/> class with the specified label and optional settings.
        /// </remarks>
        /// <param name="label">The text displayed on the button.</param>
        /// <param name="singleLine">Specifies whether the button should be displayed on a single line.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided label is null.</exception>
        public RequestLocationButton(string label, bool singleLine = false) : base(label, singleLine) { }

        /// <inheritdoc/>
        public override KeyboardButton GetButton() => new(Label)
        {
            RequestLocation = true
        };

        /// <inheritdoc/>
        public override object Clone() => new RequestLocationButton(Label, SingleLine)
        {
            ContentBuilder = ContentBuilder,
        };
    }
}