using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Editors
{
    /// <summary>
    /// Represents a buildable edit wrapper for a message.
    /// </summary>
    public interface IBuildableEdit : IBuildableMessage
    {
        /// <summary>
        /// Determines message's id that should be edited.
        /// </summary>
        public int EditMessageId { get; }

        /// <summary>
        /// Gets or sets the raw content of the edit.
        /// </summary>
        public IBuildableMessage RawContent { get; }
    }
}