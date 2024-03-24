using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Editors
{
    /// <summary>
    /// Represents a mechanism for creating an Edit Message request.
    /// </summary>
    public interface IEditWrapper : ITelegramMessage
    {
        /// <summary>
        /// Determines message's id that should be edited.
        /// </summary>
        public int EditMessageId { get; }

        /// <summary>
        /// Retrieves the content of the message.
        /// </summary>
        /// <returns>The content of the message.</returns>
        public ITelegramMessage GetContent();
    }
}