using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    /// <summary>
    /// Represents an interface for defining reply buttons used in message menus.
    /// </summary>
    public interface IReplyButton
    {
        /// <summary>
        /// Represents the label text of the reply button.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Represents a value indicating whether the reply button should be displayed in a single line.
        /// </summary>
        public bool SingleLine { get; }

        /// <summary>
        /// Generates a <see cref="KeyboardButton"/> object representing the reply button.
        /// </summary>
        /// <returns>A <see cref="KeyboardButton"/> object representing the reply button.</returns>
        public KeyboardButton GetButton();
    }
}