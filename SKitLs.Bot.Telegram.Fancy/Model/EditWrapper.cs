using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    public class EditWrapper : IBuildableMessage, IEditWrapper
    {
        public IBuildableMessage Content { get; set; }
        public int EditMessageId { get; set; }

        public EditWrapper(IBuildableMessage message, int editMessageId)
        {
            Content = message ?? throw new ArgumentNullException(nameof(message));
            EditMessageId = editMessageId;
        }

        public object Clone() => new EditWrapper((IBuildableMessage)Content.Clone(), EditMessageId);
        public string GetMessageText() => Content.GetMessageText();
    }
}
