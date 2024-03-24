using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Editors
{
    /// <summary>
    /// Represents a buildable edit message.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BuildEdit"/> class with the specified raw content and message ID to edit.
    /// </remarks>
    /// <param name="rawContent">The raw content of the message.</param>
    /// <param name="editMessageId">The ID of the message to edit.</param>
    public class BuildEdit(IBuildableMessage rawContent, int editMessageId) : IBuildableEdit
    {
        /// <summary>
        /// Gets or sets the ID of the message to edit.
        /// </summary>
        public int EditMessageId { get; set; } = editMessageId;

        /// <summary>
        /// Gets or sets the raw content of the message.
        /// </summary>
        public IBuildableMessage RawContent { get; set; } = rawContent;

        /// <inheritdoc/>
        public async Task<ITelegramMessage> BuildContentAsync(ICastedUpdate? update)
        {
            return new EditWrapper(await RawContent.BuildContentAsync(update), EditMessageId);
        }

        /// <inheritdoc/>
        public object Clone() => new BuildEdit((IBuildableMessage)RawContent.Clone(), EditMessageId);
    }
}