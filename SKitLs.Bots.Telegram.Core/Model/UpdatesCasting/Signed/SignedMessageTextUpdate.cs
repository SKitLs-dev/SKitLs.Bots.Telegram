using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    /// <summary>
    /// Casted update that represents text signed message update.
    /// Signed variant of a <see cref="AnonymMessageTextUpdate"/>
    /// </summary>
    public class SignedMessageTextUpdate : AnonymMessageTextUpdate, ISignedUpdate
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
        /// Creates a new instance of an <see cref="SignedMessageTextUpdate"/> by specifying
        /// <see cref="SignedMessageUpdate"/> as a text one.
        /// </summary>
        /// <param name="update">An instance to be specified.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageTextUpdate(SignedMessageUpdate update) : base(update)
            => Sender = update.Sender ?? throw new NullSenderException(this);

        /// <summary>
        /// Creates a new instance of an <see cref="SignedMessageTextUpdate"/>,
        /// coping data from other <see cref="AnonymMessageTextUpdate"/> and signing it with
        /// <paramref name="sender"/> instance.
        /// </summary>
        /// <param name="update">An instance to be copied.</param>
        /// <param name="sender">Casted sender instance that has raised an update.</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageTextUpdate(AnonymMessageTextUpdate update, IBotUser sender) : base(update)
            => Sender = sender ?? throw new NullSenderException(this);
    }
}