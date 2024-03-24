using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Inline
{
    /// <summary>
    /// Represents an interface for defining inline buttons used in inline message menus.
    /// </summary>
    public interface IInlineButton
    {
        /// <summary>
        /// Represents the label text of the inline button.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Represents the data of the inline button.
        /// </summary>
        public string Data { get; }

        /// <summary>
        /// Represents a value indicating whether the inline button should be displayed in a single line.
        /// </summary>
        public bool SingleLine { get; }

        /// <summary>
        /// Generates a <see cref="InlineKeyboardButton"/> object representing the inline button.
        /// </summary>
        /// <returns>A <see cref="InlineKeyboardButton"/> object representing the inline button.</returns>
        public InlineKeyboardButton GetButton();
    }
}