using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    /// <summary>
    /// Default realization of <see cref="IEditWrapper"/> that provides mechanics of creating Edit Message request.
    /// </summary>
    public class EditWrapper : IBuildableMessage, IEditWrapper
    {
        /// <summary>
        /// Determines message's id that should be edited.
        /// </summary>
        public int EditMessageId { get; set; }
        /// <summary>
        /// Represents specific message that should be pushed to <see cref="EditMessageId"/>.
        /// </summary>
        public IBuildableMessage Content { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="EditWrapper"/> with specified data.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <param name="editMessageId">Id of the message that should be updated.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EditWrapper(IBuildableMessage message, int editMessageId)
        {
            Content = message ?? throw new ArgumentNullException(nameof(message));
            EditMessageId = editMessageId;
        }

        /// <summary>
        /// Builds object's data and packs it into one text so it could be easily sent to server.
        /// </summary>
        /// <returns>Valid text, ready to be sent.</returns>
        public string GetMessageText() => Content.GetMessageText();

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() => new EditWrapper((IBuildableMessage)Content.Clone(), EditMessageId);
    }
}