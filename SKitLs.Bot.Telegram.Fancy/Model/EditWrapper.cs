using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    /// <summary>
    /// Default realization of <see cref="IEditWrapper"/> that provides mechanics of creating Edit Message request.
    /// </summary>
    public class EditWrapper : IBuildableMessage, IEditWrapper
    {
        public IBuildableMessage Content { get; set; }
        public int EditMessageId { get; set; }

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

        public object Clone() => new EditWrapper((IBuildableMessage)Content.Clone(), EditMessageId);
        public string GetMessageText() => Content.GetMessageText();
    }
}