using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Editors
{
    /// <summary>
    /// Provides extension methods for editing messages.
    /// </summary>
    public static class EditExtensions
    {
        /// <summary>
        /// Creates a new buildable edit message with the specified raw content and message ID obtained from the provided update.
        /// </summary>
        /// <param name="buildable">The raw content of the message.</param>
        /// <param name="update">The update triggering the message.</param>
        /// <returns>A buildable edit message.</returns>
        public static IBuildableEdit Edit(this IBuildableMessage buildable, IMessageTriggered update) => new BuildEdit(buildable, update.TriggerMessageId);

        /// <summary>
        /// Creates a new buildable edit message with the specified raw content and message ID.
        /// </summary>
        /// <param name="buildable">The raw content of the message.</param>
        /// <param name="editMessageId">The ID of the message to edit.</param>
        /// <returns>A buildable edit message.</returns>
        public static IBuildableEdit Edit(this IBuildableMessage buildable, int editMessageId) => new BuildEdit(buildable, editMessageId);
    }
}