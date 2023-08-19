using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using System.Reflection.Metadata.Ecma335;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    /// <summary>
    /// Default implementation of the <see cref="IEditWrapper"/> interface, providing mechanics for creating Edit Message requests.
    /// </summary>
    public class EditWrapper : TelegramTextMessage, IEditWrapper
    {
        /// <summary>
        /// Gets or sets the ID of the message that should be edited.
        /// </summary>
        public int EditMessageId { get; set; }

        /// <summary>
        /// Gets or sets the specific message content that should be pushed to a message with <see cref="EditMessageId"/> ID.
        /// </summary>
        public ITelegramMessage Content { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="EditWrapper"/> class with the specified text and edit message ID.
        /// </summary>
        /// <param name="messageText">The text to print.</param>
        /// <param name="editMessageId">The ID of the message that should be updated.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EditWrapper(string messageText, int editMessageId) : this(new TelegramTextMessage(messageText), editMessageId) { }
        /// <summary>
        /// Creates a new instance of the <see cref="EditWrapper"/> class with the specified message and edit message ID.
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <param name="editMessageId">The ID of the message that should be updated.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EditWrapper(ITelegramMessage message, int editMessageId)
        {
            Content = message ?? throw new ArgumentNullException(nameof(message));
            EditMessageId = editMessageId;
        }

        public static async Task<EditWrapper> FromBuildable(IBuildableMessage message, SignedCallbackUpdate update) => new(await message.BuildContentAsync(update), update.TriggerMessageId);

        /// <inheritdoc/>
        public override string GetMessageText() => Content.GetMessageText();

        /// <inheritdoc/>
        public override object Clone() => new EditWrapper((ITelegramMessage)Content.Clone(), EditMessageId);
    }
}