using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim
{
    /// <summary>
    /// Casted update that represents text message update. Anonymous.
    /// Signed as <see cref="SignedMessageTextUpdate"/>
    /// </summary>
    public class AnonimMessageTextUpdate : AnonimMessageUpdate
    {
        /// <summary>
        /// Incoming message's text data.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Creates a new instance of an <see cref="AnonimMessageTextUpdate"/> by specifing
        /// <see cref="AnonimMessageUpdate"/> as a text one.
        /// </summary>
        /// <param name="update">An instance to be sepcified</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonimMessageTextUpdate(AnonimMessageUpdate update) : base(update)
            => Text = Message.Text ?? throw new UpdateCastingException("Message Update: Text", update.OriginalSource.Id);

        /// <summary>
        /// Creates a new instance of an <see cref="AnonimMessageTextUpdate"/> by making anonymous
        /// <see cref="SignedMessageUpdate"/> and specifing it as a text one.
        /// </summary>
        /// <param name="update">An instance to be sepcified</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonimMessageTextUpdate(SignedMessageUpdate update) : this((AnonimMessageUpdate)update) { }
    }
}