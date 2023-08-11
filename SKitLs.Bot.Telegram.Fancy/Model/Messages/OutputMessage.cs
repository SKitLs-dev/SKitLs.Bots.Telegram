using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    /// <summary>
    /// Abstract base realization of <see cref="IOutputMessage"/>.
    /// Represents an advanced message that can be processed by <see cref="AdvancedDeliverySystem"/>.
    /// </summary>
    /// <typeparam name="TMessage">Used to specify the type of the message for which the ContentBuilder is defined.
    /// This allows dynamic message construction.</typeparam>
    public abstract class OutputMessage<TMessage> : IOutputMessage where TMessage : class, IOutputMessage
    {
        /// <inheritdoc/>
        public virtual ParseMode ParseMode { get; set; }

        /// <inheritdoc/>
        public virtual int ReplyToMessageId { get; set; }

        /// <inheritdoc/>
        public virtual bool DisableWebPagePreview { get; set; }

        /// <inheritdoc/>
        public virtual bool DisableNotification { get; set; }

        /// <inheritdoc/>
        public virtual bool ProtectContent { get; set; }

        /// <inheritdoc/>
        public virtual bool AllowSendingWithoutReply { get; set; }

        /// <inheritdoc/>
        public virtual IMessageMenu? Menu { get; set; }

        /// <summary>
        /// <b>Optional.</b> Represents the content builder for this message.
        /// </summary>
        public virtual ContentBuilder<TMessage>? ContentBuilder { get; set; }

        /// <summary>
        /// Basic constructor for the <see cref="OutputMessage{TMessage}"/> class.
        /// </summary>
        public OutputMessage() { }
        /// <summary>
        /// Constructor for the <see cref="OutputMessage{TMessage}"/> class with a specified menu.
        /// </summary>
        /// <param name="menu">The menu associated with the message.</param>
        public OutputMessage(IMessageMenu menu) => Menu = menu ?? throw new ArgumentNullException(nameof(menu));
        /// <summary>
        /// Copy constructor for the <see cref="OutputMessage{TMessage}"/> class.
        /// </summary>
        /// <param name="other">The message to be copied.</param>
        public OutputMessage(IOutputMessage other)
        {
            ReplyToMessageId = other.ReplyToMessageId;
            Menu = other.Menu;
            ParseMode = other.ParseMode;
            DisableWebPagePreview = other.DisableWebPagePreview;
            DisableNotification = other.DisableNotification;
            ProtectContent = other.ProtectContent;
            AllowSendingWithoutReply = other.AllowSendingWithoutReply;
        }

        /// <summary>
        /// Allows dynamically updating the message's parse mode.
        /// </summary>
        /// <param name="mode">The new parse mode value.</param>
        /// <returns>The current instance with the updated parse mode.</returns>
        public OutputMessage<TMessage> UseParseMode(ParseMode mode)
        {
            ParseMode = mode;
            return this;
        }
        /// <summary>
        /// Sets whether notifications for the message should be disabled.
        /// </summary>
        /// <param name="disableNotification">A value indicating whether to disable notifications.</param>
        /// <returns>The current instance with the updated notification settings.</returns>
        public OutputMessage<TMessage> Silent(bool disableNotification)
        {
            DisableNotification = disableNotification;
            return this;
        }
        /// <summary>
        /// Sets the message to be a reply to a specified message ID.
        /// </summary>
        /// <param name="replyToMessageId">The ID of the message to reply to.</param>
        /// <param name="allowSendingWithoutReply">A value indicating whether to allow sending the message without a reply.</param>
        /// <returns>The current instance with the updated reply settings.</returns>
        public OutputMessage<TMessage> ReplyTo(int replyToMessageId, bool allowSendingWithoutReply = true)
        {
            ReplyToMessageId = replyToMessageId;
            AllowSendingWithoutReply = allowSendingWithoutReply;
            return this;
        }

        /// <inheritdoc/>
        public abstract Task<ITelegramMessage> BuildContentAsync(ICastedUpdate? update);

        /// <inheritdoc/>
        public abstract object Clone();
    }
}