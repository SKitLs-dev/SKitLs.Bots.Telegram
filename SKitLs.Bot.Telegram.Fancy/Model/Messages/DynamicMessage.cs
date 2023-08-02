using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    /// <summary>
    /// Represents a dynamic message with flexible content generation based on the provided update.
    /// This class implements both the <see cref="IOutputMessage"/> and <see cref="IDynamicMessage"/> interfaces.
    /// </summary>
    public class DynamicMessage : IOutputMessage, IDynamicMessage
    {
        /// <summary>
        /// Determines message id that current message should reply to.
        /// </summary>
        public int ReplyToMessageId { get; set; }
        /// <summary>
        /// Determines message's parse mode.
        /// </summary>
        public ParseMode? ParseMode { get; set; }
        /// <summary>
        /// Determines message's menu.
        /// </summary>
        public IMesMenu? Menu { get; set; }

        /// <summary>
        /// Represents specific method that can generate message's content, based on incoming update.
        /// </summary>
        public Func<ISignedUpdate?, IOutputMessage> MessageBuilder { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicMessage"/> with the specified message building delegate.
        /// </summary>
        /// <param name="builder">The function delegate responsible for constructing the output message.</param>
        public DynamicMessage(Func<ISignedUpdate?, IOutputMessage> builder) => MessageBuilder = builder;
        /// <summary>
        /// Initializes a new instance of <see cref="DynamicMessage"/> by cloning it from another one.
        /// </summary>
        /// <param name="other"><see cref="DynamicMessage"/> to be cloned.</param>
        public DynamicMessage(DynamicMessage other) => MessageBuilder = (Func<ISignedUpdate?, IOutputMessage>)other.MessageBuilder.Clone();

        /// <summary>
        /// Generates new message content, based on an incoming <paramref name="update"/>.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>Updated with <paramref name="update"/> message.</returns>
        public IOutputMessage BuildWith(ISignedUpdate? update)
        {
            var message = MessageBuilder(update);
            message.ReplyToMessageId = ReplyToMessageId;
            message.ParseMode = ParseMode;
            message.Menu = Menu;
            return message;
        }

        /// <summary>
        /// Builds object's data and packs it into one text so it could be easily sent to server.
        /// </summary>
        /// <returns>Valid text, ready to be sent.</returns>
        public string GetMessageText() => MessageBuilder(null).GetMessageText();

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() => new DynamicMessage(this);
    }
}