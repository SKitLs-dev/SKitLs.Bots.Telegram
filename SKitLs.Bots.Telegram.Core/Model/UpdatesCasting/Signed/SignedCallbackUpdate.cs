using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Prototype;
using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    /// <summary>
    /// Casted update that represents default signed callback update.
    /// </summary>
    public class SignedCallbackUpdate : CastedUpdate, ISignedUpdate
    {
        /// <summary>
        /// Casted sender instance that has raised an update.
        /// <para>
        /// Generated via <see cref="ChatScanner.UsersManager"/>
        /// or <see cref="ChatScanner.GetDefaultBotUser"/> mechanisms
        /// of a <see cref="ChatScanner"/> class by default.
        /// </para>
        /// </summary>
        public IBotUser Sender { get; init; }
        /// <summary>
        /// Callback query that has raised an update.
        /// </summary>
        public CallbackQuery Callback { get; init; }
        /// <summary>
        /// Incoming message's callback data.
        /// </summary>
        public string Data { get; init; }
        /// <summary>
        /// Message instance that has raised an update.
        /// </summary>
        public Message Message { get; init; }
        /// <summary>
        /// Id of a message that have raised current update.
        /// </summary>
        public int TriggerMessageId => Message.MessageId;

        /// <summary>
        /// Creates a new instance of an <see cref="SignedMessageUpdate"/>, using specific data.
        /// </summary>
        /// <param name="chatScanner">Chat Scanner that has raised casted update.</param>
        /// <param name="source">Original telegram update. Not casted, contains null values.</param>
        /// <param name="chatId">ID of a chat that has raised updated.</param>
        /// <param name="sender">Casted sender instance that has raised an update.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedCallbackUpdate(ChatScanner chatScanner, Update source, long chatId, IBotUser sender)
            : base(chatScanner, source, chatId)
        {
            Callback = source.CallbackQuery ?? throw new UpdateCastingException(source.Id, "Callback: Query");
            Message = source.CallbackQuery.Message ?? throw new UpdateCastingException(source.Id, "Callback: Message");
            Data = source.CallbackQuery.Data ?? throw new UpdateCastingException(source.Id, "Callback: Data");
            Sender = sender ?? throw new NullSenderException(this);
        }

        /// <summary>
        /// Creates a new instance of a <see cref="SignedMessageUpdate"/>,
        /// coping data from other <see cref="ICastedUpdate"/> and signing it with
        /// <paramref name="sender"/> instance.
        /// </summary>
        /// <param name="update">An instance to be copied.</param>
        /// <param name="sender">Casted sender instance that has raised an update.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedCallbackUpdate(ICastedUpdate update, IBotUser sender) : this(update.ChatScanner, update.OriginalSource, update.ChatId, sender) { }
    }
}