using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Inline
{
    /// <summary>
    /// Represents an <see cref="InlineButton"/> that opens a URL when pressed.
    /// </summary>
    public class UrlButton : InlineButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlButton"/> class with the specified label text, URL, and optional settings.
        /// </summary>
        /// <param name="label">The text displayed on the button.</param>
        /// <param name="url">The URL to which the user is redirected when the button is pressed.</param>
        /// <param name="singleLine">Optional. Indicates whether the button should be displayed on a single line.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided label or URL is null.</exception>
        public UrlButton(string label, string url, bool singleLine = false) : base(label, url, singleLine) { }

        /// <inheritdoc/>
        public override InlineKeyboardButton GetButton() => InlineKeyboardButton.WithUrl(Label, Data);
    }
}