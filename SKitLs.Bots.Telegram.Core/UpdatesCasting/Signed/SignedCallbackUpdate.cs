using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.Users;
using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    /// <summary>
    /// Represents a casted update that represents a default signed callback update.
    /// </summary>
    public class SignedCallbackUpdate : CastedUpdate, ISignedUpdate, IMessageTriggered
    {
        /// <inheritdoc/>
        public IBotUser Sender { get; init; }

        /// <summary>
        /// Gets the callback query that has raised an update.
        /// </summary>
        public CallbackQuery Callback { get; init; }

        /// <summary>
        /// Gets the incoming message's callback data.
        /// </summary>
        public string Data { get; init; }

        /// <summary>
        /// Gets the message instance that has raised an update.
        /// </summary>
        public Message Message { get; init; }

        /// <inheritdoc/>
        public int TriggerMessageId => Message.MessageId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedCallbackUpdate"/> class using specific data.
        /// </summary>
        /// <param name="chatScanner">The chat scanner that has raised the casted update.</param>
        /// <param name="source">The original Telegram update, not casted, containing null values.</param>
        /// <param name="chatId">The ID of the chat that has raised the update.</param>
        /// <param name="sender">The casted sender instance that has raised the update.</param>
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
        /// Initializes a new instance of the <see cref="SignedCallbackUpdate"/> class,
        /// copying data from another <see cref="ICastedUpdate"/> and signing it with the specified sender instance.
        /// </summary>
        /// <param name="update">The instance to be copied.</param>
        /// <param name="sender">The casted sender instance that has raised the update.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedCallbackUpdate(ICastedUpdate update, IBotUser sender) : this(update.ChatScanner, update.OriginalSource, update.ChatId, sender) { }
    }
}