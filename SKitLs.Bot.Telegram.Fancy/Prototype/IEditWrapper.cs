using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    public interface IEditWrapper : IBuildableMessage
    {
        public IBuildableMessage Content { get; }
        public int EditMessageId { get; }
    }
}