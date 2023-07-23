using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim
{
    /// <summary>
    /// Casted update that represents default message update. Anonymous.
    /// Signed as <see cref="Signed.SignedMessageUpdate"/>
    /// </summary>
    public class AnonimMessageUpdate : CastedUpdate
    {
        /// <summary>
        /// Message instance that has raised an update.
        /// </summary>
        public Message Message { get; private set; }
        /// <summary>
        /// ID of a message that has raised current update.
        /// </summary>
        public int TriggerMessageId => Message.MessageId;

        /// <summary>
        /// Creates a new instance of an <see cref="AnonimMessageUpdate"/>, using specific data.
        /// </summary>
        /// <param name="chatScanner">Chat Scanner that has raised casted update</param>
        /// <param name="source">Original telegram update. Not casted, contains null values</param>
        /// <param name="chatId">ID of a chat that has raised updated</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonimMessageUpdate(ChatScanner chatScanner, Update source, long chatId) : base(chatScanner, source, chatId)
            => Message = source.Message ?? throw new UpdateCastingException(source.Id, "Message Update");

        /// <summary>
        /// Creates a new instance of an <see cref="AnonimMessageUpdate"/>,
        /// coping data from other <see cref="ICastedUpdate"/>
        /// </summary>
        /// <param name="update">An instance to be copied</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonimMessageUpdate(ICastedUpdate update) : this(update.ChatScanner, update.OriginalSource, update.ChatId) { }
    }
}