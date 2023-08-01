using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym
{
    /// <summary>
    /// Casted update that represents text message update. Anonymous.
    /// Signed as <see cref="SignedMessageTextUpdate"/>
    /// </summary>
    public class AnonymMessageTextUpdate : AnonymMessageUpdate
    {
        /// <summary>
        /// Incoming message's text data.
        /// </summary>
        public string Text { get; init; }

        /// <summary>
        /// Creates a new instance of an <see cref="AnonymMessageTextUpdate"/> by specifying
        /// <see cref="AnonymMessageUpdate"/> as a text one.
        /// </summary>
        /// <param name="update">An instance to be specified.</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonymMessageTextUpdate(AnonymMessageUpdate update) : base(update)
            => Text = Message.Text ?? throw new UpdateCastingException(update.OriginalSource.Id, "Message Update: Text");

        /// <summary>
        /// Creates a new instance of an <see cref="AnonymMessageTextUpdate"/> by making anonymous
        /// <see cref="SignedMessageUpdate"/> and specifying it as a text one.
        /// </summary>
        /// <param name="update">An instance to be specified.</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonymMessageTextUpdate(SignedMessageUpdate update) : this((AnonymMessageUpdate)update) { }
    }
}