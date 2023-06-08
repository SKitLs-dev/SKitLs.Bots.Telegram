using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    /// <summary>
    /// Casted update that represents text signed message update.
    /// Signed variants of a <see cref="AnonimMessageTextUpdate"/>
    /// </summary>
    public class SignedMessageTextUpdate : AnonimMessageTextUpdate, ISignedUpdate
    {
        public IBotUser Sender { get; private set; }

        /// <summary>
        /// Creates a new instance of an <see cref="SignedMessageTextUpdate"/> by specifing
        /// <see cref="SignedMessageUpdate"/> as a text one.
        /// </summary>
        /// <param name="update">An instance to be sepcified</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageTextUpdate(SignedMessageUpdate update) : base(update)
            => Sender = update.Sender ?? throw new NullSenderException();

        /// <summary>
        /// Creates a new instance of an <see cref="SignedMessageTextUpdate"/>,
        /// coping data from other <see cref="AnonimMessageTextUpdate"/> and signing it with
        /// <paramref name="sender"/> instance.
        /// </summary>
        /// <param name="update">An instance to be copied</param>
        /// <param name="sender">Casted sender instance that has raised an update</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageTextUpdate(AnonimMessageTextUpdate update, IBotUser sender) : base(update)
            => Sender = sender ?? throw new NullSenderException();
    }
}