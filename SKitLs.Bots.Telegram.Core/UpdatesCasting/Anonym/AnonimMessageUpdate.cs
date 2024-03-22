using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym
{
    /// <summary>
    /// Represents a casted update that corresponds to a default message update and is anonymous.
    /// Signed as <see cref="Signed.SignedMessageUpdate"/>.
    /// </summary>
    public class AnonymMessageUpdate : CastedUpdate, IMessageTriggered
    {
        /// <summary>
        /// Gets the message instance that raised the update.
        /// </summary>
        public Message Message { get; init; }

        /// <inheritdoc/>
        public int TriggerMessageId => Message.MessageId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymMessageUpdate"/> class with specific data.
        /// </summary>
        /// <param name="chatScanner">The Chat Scanner that raised the casted update.</param>
        /// <param name="source">The original Telegram update. Not casted, may contain null values.</param>
        /// <param name="chatId">The ID of the chat that raised the update.</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonymMessageUpdate(ChatScanner chatScanner, Update source, long chatId) : base(chatScanner, source, chatId)
            => Message = source.Message ?? throw new UpdateCastingException(source.Id, "Message Update");

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymMessageUpdate"/> class, copying data from another <see cref="ICastedUpdate"/>.
        /// </summary>
        /// <param name="update">The instance to be copied.</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonymMessageUpdate(ICastedUpdate update) : this(update.ChatScanner, update.OriginalSource, update.ChatId) { }
    }
}