using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym
{
    // XML-Doc Update
    /// <summary>
    /// Casted update that represents default message update. Anonymous.
    /// Signed as <see cref="Signed.SignedMessageUpdate"/>
    /// </summary>
    public class AnonymMessageUpdate : CastedUpdate, IMessageTriggered
    {
        /// <summary>
        /// Message instance that has raised an update.
        /// </summary>
        public Message Message { get; init; }

        /// <inheritdoc/>
        public int TriggerMessageId => Message.MessageId;

        /// <summary>
        /// Creates a new instance of an <see cref="AnonymMessageUpdate"/>, using specific data.
        /// </summary>
        /// <param name="chatScanner">Chat Scanner that has raised casted update.</param>
        /// <param name="source">Original telegram update. Not casted, contains null values.</param>
        /// <param name="chatId">ID of a chat that has raised updated.</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonymMessageUpdate(ChatScanner chatScanner, Update source, long chatId) : base(chatScanner, source, chatId)
            => Message = source.Message ?? throw new UpdateCastingException(source.Id, "Message Update");

        /// <summary>
        /// Creates a new instance of an <see cref="AnonymMessageUpdate"/>,
        /// coping data from other <see cref="ICastedUpdate"/>
        /// </summary>
        /// <param name="update">An instance to be copied.</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonymMessageUpdate(ICastedUpdate update) : this(update.ChatScanner, update.OriginalSource, update.ChatId) { }
    }
}