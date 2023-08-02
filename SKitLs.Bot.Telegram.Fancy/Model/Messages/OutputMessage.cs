using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    /// <summary>
    /// Abstract base realization of <see cref="IOutputMessage"/>. Represents an advanced message that can be
    /// processed by <see cref="AdvancedDeliverySystem"/>.
    /// </summary>
    public abstract class OutputMessage : IOutputMessage
    {
        /// <summary>
        /// Determines message id that current message should reply to.
        /// </summary>
        public int ReplyToMessageId { get; set; }
        /// <summary>
        /// Determines message's parse mode.
        /// </summary>
        public ParseMode? ParseMode { get; set; }
        /// <summary>
        /// Determines message's menu.
        /// </summary>
        public IMesMenu? Menu { get; set; }

        /// <summary>
        /// Basic constructor for <see cref="OutputMessage"/>.
        /// </summary>
        public OutputMessage() { }
        /// <summary>
        /// Copying constructor for <see cref="OutputMessage"/>.
        /// </summary>
        /// <param name="other">Message to be copied.</param>
        public OutputMessage(IOutputMessage other)
        {
            ReplyToMessageId = other.ReplyToMessageId;
            Menu = other.Menu;
            ParseMode = other.ParseMode;
        }

        /// <summary>
        /// Allows to dynamically update message's parse mode.
        /// </summary>
        /// <param name="mode">New mode's value.</param>
        /// <returns>Current instance with updated parse mode.</returns>
        public OutputMessage UseParseMode(ParseMode mode)
        {
            ParseMode = mode;
            return this;
        }
        /// <summary>
        /// Allows to dynamically update message's menu.
        /// </summary>
        /// <param name="menu">Menu to be implemented.</param>
        /// <returns>Current instance with updated menu.</returns>
        public OutputMessage AddMenu(IMesMenu? menu)
        {
            Menu = menu;
            return this;
        }

        /// <summary>
        /// Builds object's data and packs it into one text so it could be easily sent to server.
        /// </summary>
        /// <returns>Valid text, ready to be sent.</returns>
        public abstract string GetMessageText();

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public abstract object Clone();
    }
}
