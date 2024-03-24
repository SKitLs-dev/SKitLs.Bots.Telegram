using SKitLs.Bots.Telegram.AdvancedMessages.Editors;
using SKitLs.Bots.Telegram.Core.DeliverySystem;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery
{
    /// <summary>
    /// Advanced realization of <see cref="IDeliveryService"/> that works with string and simple <see cref="IBuildableMessage"/> messages.
    /// </summary>
    public class AdvancedDeliveryService : DefaultDeliveryService
    {
        /// <inheritdoc/>s
        public override async Task<DeliveryResponse> SendMessageToChatAsync(long chatId, ITelegramMessage message, CancellationTokenSource? cts = null)
        {
            cts ??= new();
            if (message is IEditWrapper edit)
                return await HandleEditAsync(chatId, edit, cts);
            else
                return await base.SendMessageToChatAsync(chatId, message, cts);
        }

        /// <summary>
        /// Asynchronously handles the editing of a message.
        /// </summary>
        /// <param name="chatId">The chat ID.</param>
        /// <param name="editWrapper">The edit wrapper containing the message content to be edited to.</param>
        /// <param name="cts">The cancellation token source.</param>
        /// <returns>A task representing the asynchronous operation with a <see cref="DeliveryResponse"/>.</returns>
        protected virtual async Task<DeliveryResponse> HandleEditAsync(long chatId, IEditWrapper editWrapper, CancellationTokenSource cts)
        {
            //if (message.Content is IOutputMessage output) return await EditOutputAsync(output, chatId, message.EditMessageId, cts);
            //else
            return await EditTextAsync(chatId, editWrapper.EditMessageId, editWrapper.GetContent(), cts);
        }

        /// <summary>
        /// Asynchronously edits a message.
        /// </summary>
        /// <param name="chatId">The chat ID.</param>
        /// <param name="messageId">The message ID.</param>
        /// <param name="message">The message content to be edited to.</param>
        /// <param name="cts">The cancellation token source.</param>
        /// <returns>A task representing the asynchronous operation with a <see cref="DeliveryResponse"/>.</returns>
        protected virtual async Task<DeliveryResponse> EditTextAsync(long chatId, int messageId, ITelegramMessage message, CancellationTokenSource cts)
        {
            try
            {
                var text = Owner.Settings.MakeDeliverySafe && message.ParseMode is not null
                    ? MakeParseSafe(message.GetMessageText(), message.ParseMode.Value)
                    : message.GetMessageText();

                var resMessage = await Bot.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: text,
                    parseMode: message.ParseMode,
                    disableWebPagePreview: message.DisableWebPagePreview,
                    replyMarkup: (InlineKeyboardMarkup?)message.GetReplyMarkup(),
                    cancellationToken: cts.Token);

                return new DeliveryResponse(resMessage);
            }
            catch (Exception e)
            {
                cts.Cancel();
                return new DeliveryResponse(e);
            }
        }
    }
}