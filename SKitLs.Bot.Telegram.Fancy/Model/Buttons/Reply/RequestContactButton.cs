using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Reply
{
    /// <summary>
    /// <b>Private chats only.</b> Represents a button that requests the
    /// <see href="https://core.telegram.org/bots/api#keyboardbutton">user's contact information</see> (phone number) when pressed.
    /// </summary>
    public class RequestContactButton : ReplyButton
    {
        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of the <see cref="RequestLocationButton"/> class with the specified label and optional settings.
        /// </summary>
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