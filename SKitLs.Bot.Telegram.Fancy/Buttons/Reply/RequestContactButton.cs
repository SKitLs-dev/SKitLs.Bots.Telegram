using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Reply
{
    /// <summary>
    /// <b>Private chats only.</b> Represents a button that requests the
    /// <see href="https://core.telegram.org/bots/api#keyboardbutton">user's contact information</see> (phone number) when pressed.
    /// </summary>
    public class RequestContactButton : ReplyButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLocationButton"/> class with the specified label and optional settings.
        /// </summary>
        /// <param name="label">The text displayed on the button.</param>
        /// <param name="singleLine">Specifies whether the button should be displayed on a single line.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided label is null.</exception>
        public RequestContactButton(string label, bool singleLine = false) : base(label, singleLine) { }

        /// <inheritdoc/>
        public override KeyboardButton GetButton() => new(Label)
        {
            RequestContact = true
        };

        /// <inheritdoc/>
        public override object Clone() => new RequestContactButton(Label, SingleLine)
        {
            ContentBuilder = ContentBuilder,
        };
    }
}