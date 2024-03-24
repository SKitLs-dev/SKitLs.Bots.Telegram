using SKitLs.Bots.Telegram.Core.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Editors
{
    /// <summary>
    /// Default implementation of the <see cref="IEditWrapper"/> interface, providing mechanics for creating Edit Message requests.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EditWrapper"/> class with the specified message and edit message ID.
    /// </remarks>
    /// <param name="message">The message to print.</param>
    /// <param name="editMessageId">The ID of the message that should be updated.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public class EditWrapper(ITelegramMessage message, int editMessageId) : TelegramTextMessage, IEditWrapper
    {
        /// <summary>
        /// Gets or sets the ID of the message that should be edited.
        /// </summary>
        public int EditMessageId { get; set; } = editMessageId;

        /// <summary>
        /// Gets or sets the specific message content that should be pushed to a message with <see cref="EditMessageId"/> ID.
        /// </summary>
        public ITelegramMessage Content { get; set; } = message ?? throw new ArgumentNullException(nameof(message));

        /// <summary>
        /// Initializes a new instance of the <see cref="EditWrapper"/> class with the specified text and edit message ID.
        /// </summary>
        /// <param name="messageText">The text to print.</param>
        /// <param name="editMessageId">The ID of the message that should be updated.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EditWrapper(string messageText, int editMessageId) : this(new TelegramTextMessage(messageText), editMessageId) { }

        /// <inheritdoc/>
        public ITelegramMessage GetContent() => Content;

        /// <inheritdoc/>
        public override object Clone() => new EditWrapper((ITelegramMessage)Content.Clone(), EditMessageId);
    }
}