﻿using SKitLs.Bots.Telegram.Core.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Messages.Text
{
    /// <summary>
    /// Default implementation of <see cref="IOutputMessage"/>. Derived from the abstract class <see cref="OutputMessage{TMessage}"/>.
    /// </summary>
    public class OutputMessageText : OutputMessage<OutputMessageText>
    {
        /// <summary>
        /// Get or sets the text of the message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="OutputMessageText"/> with the specified text.
        /// </summary>
        /// <param name="text">The text of the message.</param>
        public OutputMessageText(string text) => Text = text;

        /// <inheritdoc/>
        public OutputMessageText(IOutputMessage other) : base(other)
        {
            Text = other is OutputMessageText text ? (string)text.Text.Clone() : other.GetType().Name;
        }

        /// <inheritdoc/>
        public override object Clone() => new OutputMessageText(this);

        /// <inheritdoc/>
        public override async Task<ITelegramMessage> BuildContentAsync(ICastedUpdate? update)
        {
            var message = ContentBuilder is not null ? await ContentBuilder.Invoke(this, update) : this;
            var menu = Menu is not null ? await Menu.BuildContentAsync(update) : null;

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