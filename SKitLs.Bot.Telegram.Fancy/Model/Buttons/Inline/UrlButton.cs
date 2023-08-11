using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Inline
{
    /// <summary>
    /// Represents an <see cref="InlineButton"/> that opens a URL when pressed.
    /// </summary>
    public class UrlButton : InlineButton
    {
        /// <inheritdoc/>
        public UrlButton(string label, string url, bool singleLine = false) : base(label, url, singleLine) { }

        /// <inheritdoc/>
        public override InlineKeyboardButton GetButton() => InlineKeyboardButton.WithUrl(Label, Data);
    }
}