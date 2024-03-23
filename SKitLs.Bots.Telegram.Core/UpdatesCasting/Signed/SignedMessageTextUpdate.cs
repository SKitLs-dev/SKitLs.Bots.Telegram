using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Users;

namespace SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed
{
    /// <summary>
    /// Represents a casted update that represents a text signed message update.
    /// It is the signed variant of <see cref="AnonymMessageTextUpdate"/>.
    /// </summary>
    public class SignedMessageTextUpdate : AnonymMessageTextUpdate, ISignedUpdate
    {
        /// <inheritdoc/>
        public IBotUser Sender { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedMessageTextUpdate"/> class by specifying
        /// <see cref="SignedMessageUpdate"/> as a text one.
        /// </summary>
        /// <param name="update">The instance to be specified.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageTextUpdate(SignedMessageUpdate update) : base(update)
            => Sender = update.Sender ?? throw new NullSenderException(this);

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedMessageTextUpdate"/> class,
        /// copying data from another <see cref="AnonymMessageTextUpdate"/> and signing it with the specified sender instance.
        /// </summary>
        /// <param name="update">The instance to be copied.</param>
        /// <param name="sender">The casted sender instance that has raised an update.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageTextUpdate(AnonymMessageTextUpdate update, IBotUser sender) : base(update)
            => Sender = sender ?? throw new NullSenderException(this);
    }
}