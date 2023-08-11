using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text
{
    /// <summary>
    /// Default implementation of <see cref="IOutputMessage"/>. Derived from the abstract class <see cref="OutputMessage{TMessage}"/>.
    /// </summary>
    public class OutputMessageText : OutputMessage<OutputMessageText>
    {
        /// <summary>
        /// Represents the text of the message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="OutputMessageText"/> with the specified text.
        /// </summary>
        /// <param name="text">The text of the message.</param>
        public OutputMessageText(string text) => Text = text;
        
        /// <inheritdoc/>
        public OutputMessageText(IOutputMessage other) : base(other)
        {
            Text = other is OutputMessageText text ? text.Text : other.GetType().Name;
        }

        /// <inheritdoc/>
        public override object Clone() => new OutputMessageText(this);

        /// <inheritdoc/>
        public override async Task<ITelegramMessage> BuildContentAsync(ICastedUpdate? update)
        {
            var message = ContentBuilder is not null ? await ContentBuilder.Invoke(this, update) : this;
            var menu = message.Menu is IBuildableContent<IMessageMenu> buildable ? await buildable.BuildContentAsync(update) : Menu;

            return new TelegramTextMessage(message.Text)
            {
                AllowSendingWithoutReply = AllowSendingWithoutReply,
                DisableNotification = DisableNotification,
                DisableWebPagePreview = DisableWebPagePreview,
                ParseMode = ParseMode,
                ProtectContent = ProtectContent,
                ReplyMarkup = menu?.GetMarkup(),
                ReplyToMessageId = ReplyToMessageId,
            };
        }
    }
}