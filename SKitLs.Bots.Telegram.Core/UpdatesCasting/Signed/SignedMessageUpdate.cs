using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Model.Users;
using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    /// <summary>
    /// Represents a casted update that represents a default signed message update.
    /// It is the signed variant of <see cref="AnonymMessageUpdate"/>.
    /// </summary>
    public class SignedMessageUpdate : AnonymMessageUpdate, ISignedUpdate
    {
        /// <inheritdoc/>
        public IBotUser Sender { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedMessageUpdate"/> class with the specified data.
        /// </summary>
        /// <param name="chatScanner">The chat scanner that has raised the casted update.</param>
        /// <param name="source">The original Telegram update. Not casted, contains null values.</param>
        /// <param name="chatId">The ID of the chat that has raised the updated.</param>
        /// <param name="sender">The casted sender instance that has raised the update.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageUpdate(ChatScanner chatScanner, Update source, long chatId, IBotUser sender)
            : base(chatScanner, source, chatId) => Sender = sender ?? throw new NullSenderException(this);

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedMessageUpdate"/> class,
        /// copying data from another <see cref="ICastedUpdate"/> and signing it with the specified <paramref name="sender"/> instance.
        /// </summary>
        /// <param name="update">The instance to be copied.</param>
        /// <param name="sender">The casted sender instance that has raised the update.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageUpdate(ICastedUpdate update, IBotUser sender) : this(update.ChatScanner, update.OriginalSource, update.ChatId, sender) { }
    }
}