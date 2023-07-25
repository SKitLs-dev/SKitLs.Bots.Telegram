using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    /// <summary>
    /// Provides mechanics of creating Edit Message request.
    /// </summary>
    public interface IEditWrapper : IBuildableMessage
    {
        /// <summary>
        /// Determines message's id that should be edited.
        /// </summary>
        public int EditMessageId { get; }
        /// <summary>
        /// Represents specific message that should be pushed to <see cref="EditMessageId"/>.
        /// </summary>
        public IBuildableMessage Content { get; }
    }
}