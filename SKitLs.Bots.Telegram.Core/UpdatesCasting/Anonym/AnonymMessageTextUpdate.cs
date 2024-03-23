using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.UpdatesCasting.Anonym
{
    /// <summary>
    /// Represents a casted update that corresponds to a text message update and is anonymous.
    /// Signed as <see cref="SignedMessageTextUpdate"/>.
    /// </summary>
    public class AnonymMessageTextUpdate : AnonymMessageUpdate
    {
        /// <summary>
        /// Gets the incoming message's text data.
        /// </summary>
        public string Text { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymMessageTextUpdate"/> class by specifying
        /// <see cref="AnonymMessageUpdate"/> as a text one.
        /// </summary>
        /// <param name="update">The instance to be specified.</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonymMessageTextUpdate(AnonymMessageUpdate update) : base(update)
            => Text = Message.Text ?? throw new UpdateCastingException(update.OriginalSource.Id, "Message Update: Text");

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymMessageTextUpdate"/> class by making anonymous
        /// <see cref="SignedMessageUpdate"/> and specifying it as a text one.
        /// </summary>
        /// <param name="update">The instance to be specified.</param>
        /// <exception cref="UpdateCastingException"></exception>
        public AnonymMessageTextUpdate(SignedMessageUpdate update) : this((AnonymMessageUpdate)update) { }
    }
}