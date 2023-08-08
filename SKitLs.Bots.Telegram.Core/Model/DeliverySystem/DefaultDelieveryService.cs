using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem
{
    /// <summary>
    /// Represents the default implementation of <see cref="IDeliveryService"/> that handles string messages
    /// and simple <see cref="ITelegramMessage"/> messages.
    /// </summary>
    public class DefaultDeliveryService : IDeliveryService
    {
        private BotManager? _owner;
        /// <inheritdoc/>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <inheritdoc/>
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Provides quick access to the bot client by the entity's owner.
        /// </summary>
        private ITelegramBotClient Bot => Owner.Bot;

        /// <inheritdoc/>
        public bool IsParseSafe(string text, ParseMode parseMode) => parseMode switch
        {
            ParseMode.Markdown => IDeliveryService.IsMarkdownSafe(text),
            _ => true,
        };

        /// <inheritdoc/>
        public string MakeParseSafe(string text, ParseMode parseMode) => parseMode switch
        {
            ParseMode.Markdown => IDeliveryService.MakeMarkdownSafe(text),
            _ => text,
        };

        /// <inheritdoc/>
        public async Task<DeliveryResponse> AnswerSenderAsync(string message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(update.Sender.TelegramId, message, cts);

        /// <inheritdoc/>
        /// <remarks>
        /// Automatically builds the content of the <paramref name="message"/> if it is the <see cref="IBuildableMessage"/> one.
        /// </remarks>
        public async Task<DeliveryResponse> AnswerSenderAsync(ITelegramMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(update.Sender.TelegramId, message is IBuildableMessage buildable ? await buildable.BuildContentAsync(update) : message, cts);

        /// <inheritdoc/>
        public async Task<DeliveryResponse> SendMessageToChatAsync(long chatId, string message, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(chatId, new TelegramTextMessage(message), cts);

        /// <inheritdoc/>
        public async Task<DeliveryResponse> SendMessageToChatAsync(long chatId, ITelegramMessage message, CancellationTokenSource? cts = null)
        {
            cts ??= new();
            try
            {
                var text = Owner.Settings.MakeDeliverySafe
                    ? MakeParseSafe(message.GetMessageText(), message.ParseMode)
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