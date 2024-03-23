using SKitLs.Bots.Telegram.Core.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Services;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.DeliverySystem
{
    /// <summary>
    /// Represents the default implementation of <see cref="IDeliveryService"/> that handles string messages
    /// and simple <see cref="ITelegramMessage"/> messages.
    /// </summary>
    public class DefaultDeliveryService : BotServiceBase, IDeliveryService
    {
        /// <summary>
        /// Provides quick access to the bot client by the entity's owner.
        /// </summary>
        protected ITelegramBotClient Bot => Owner.Bot;

        /// <inheritdoc/>
        public virtual bool IsParseSafe(string text, ParseMode parseMode) => parseMode switch
        {
            ParseMode.Markdown => IDeliveryService.IsMarkdownSafe(text),
            _ => true,
        };

        /// <inheritdoc/>
        public virtual string MakeParseSafe(string text, ParseMode parseMode) => parseMode switch
        {
            ParseMode.Markdown => IDeliveryService.MakeMarkdownSafe(text),
            _ => text,
        };

        /// <inheritdoc/>
        public virtual async Task<DeliveryResponse> AnswerSenderAsync(IBuildableMessage buildable, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(update.Sender.TelegramId, buildable, update, cts);

        /// <inheritdoc/>
        public virtual async Task<DeliveryResponse> AnswerSenderAsync(string message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(update.Sender.TelegramId, message, cts);

        /// <inheritdoc/>
        public virtual async Task<DeliveryResponse> AnswerSenderAsync(ITelegramMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(update.Sender.TelegramId, message, cts);

        /// <inheritdoc/>
        public virtual async Task<DeliveryResponse> SendMessageToChatAsync(long chatId, IBuildableMessage buildable, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(chatId, await buildable.BuildContentAsync(update), cts);

        /// <inheritdoc/>
        public virtual async Task<DeliveryResponse> SendMessageToChatAsync(long chatId, string message, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(chatId, new TelegramTextMessage(message), cts);

        /// <inheritdoc/>
        public virtual async Task<DeliveryResponse> SendMessageToChatAsync(long chatId, ITelegramMessage message, CancellationTokenSource? cts = null)
        {
            cts ??= new();
            try
            {
                var text = Owner.Settings.MakeDeliverySafe && message.ParseMode is not null
                    ? MakeParseSafe(message.GetMessageText(), message.ParseMode.Value)
                    : message.GetMessageText();

                var resMessage = await Bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    parseMode: message.ParseMode,
                    disableWebPagePreview: message.DisableWebPagePreview,
                    disableNotification: message.DisableNotification,
                    protectContent: message.ProtectContent,
                    replyToMessageId: message.ReplyToMessageId,
                    allowSendingWithoutReply: message.AllowSendingWithoutReply,
                    replyMarkup: message.GetReplyMarkup(),
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