using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    /// <summary>
    /// Represents a dynamic message with flexible content generation based on the provided update.
    /// This class implements both the <see cref="IOutputMessage"/> and <see cref="IDynamicMessage"/> interfaces.
    /// </summary>
    [Obsolete($"Replaced with {nameof(IBuildableMessage)} since .Core [v2.1]")]
    public class DynamicMessage : IOutputMessage, IDynamicMessage
    {
        /// <summary>
        /// Determines message id that current message should reply to.
        /// </summary>
        public int ReplyToMessageId { get; set; }

        /// <summary>
        /// <b>Not Implemented</b>
        /// </summary>
        public ParseMode ParseMode { get; set; }

        /// <summary>
        /// Determines message's menu.
        /// </summary>
        public IMessageMenu? Menu { get; set; }

        /// <summary>
        /// Represents specific method that can generate message's content, based on incoming update.
        /// </summary>
        public Func<ISignedUpdate?, IOutputMessage> MessageBuilder { get; set; }

        /// <summary>
        /// <b>Not Implemented</b>
        /// </summary>
        public bool DisableWebPagePreview => throw new NotImplementedException();

        /// <summary>
        /// <b>Not Implemented</b>
        /// </summary>
        public bool DisableNotification => throw new NotImplementedException();

        /// <summary>
        /// <b>Not Implemented</b>
        /// </summary>
        public bool ProtectContent => throw new NotImplementedException();

        /// <summary>
        /// <b>Not Implemented</b>
        /// </summary>
        public bool AllowSendingWithoutReply => throw new NotImplementedException();

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
        /// <b>Not Implemented</b>
        /// </summary>
        public Task BuildWith(ISignedUpdate? update) => throw new NotImplementedException();

        /// <summary>
        /// Builds object's data and packs it into one text so it could be easily sent to server.
        /// </summary>
        /// <returns>Valid text, ready to be sent.</returns>
        public string GetMessageText() => throw new NotImplementedException();

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() => new DynamicMessage(this);

        /// <summary>
        /// <b>Not Implemented</b>
        /// </summary>
        public IReplyMarkup? GetReplyMarkup()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <b>Not Implemented</b>
        /// </summary>
        public Task<ITelegramMessage> BuildContentAsync(ICastedUpdate update)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <b>Not Implemented</b>
        /// </summary>
        IOutputMessage IDynamicMessage.BuildWith(ISignedUpdate? update)
        {
            throw new NotImplementedException();
        }
    }
}