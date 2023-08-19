using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TEnums = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery
{
    /// <summary>
    /// Advanced realization of <see cref="IDeliveryService"/> that works with string and simple <see cref="IBuildableMessage"/> messages.
    /// </summary>
    public class AdvancedDeliverySystem : IDeliveryService
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
        public async Task<DeliveryResponse> AnswerSenderAsync(ITelegramMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(update.Sender.TelegramId, message is IBuildableMessage buildable ? await buildable.BuildContentAsync(update) : message, cts);

        /// <inheritdoc/>
        public async Task<DeliveryResponse> SendMessageToChatAsync(long chatId, string message, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(chatId, new TelegramTextMessage(message), cts);

        /// <inheritdoc/>s
        public async Task<DeliveryResponse> SendMessageToChatAsync(long chatId, ITelegramMessage message, CancellationTokenSource? cts = null)
        {
            cts ??= new();
            if (message is IEditWrapper edit) return await HandleEditAsync(edit, chatId, cts);
            else return await SendTexMessageAsync(chatId, message, cts);
        }

        private async Task<DeliveryResponse> SendTexMessageAsync(long chatId, ITelegramMessage message, CancellationTokenSource cts)
        {
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

        private async Task<DeliveryResponse> HandleEditAsync(IEditWrapper message, long chatId, CancellationTokenSource cts)
        {
            //if (message.Content is IOutputMessage output) return await EditOutputAsync(output, chatId, message.EditMessageId, cts);
            //else
            return await EditTextAsync(chatId, message.EditMessageId, message.Content, cts);
        }
        private async Task<DeliveryResponse> EditTextAsync(long chatId, int mesId, ITelegramMessage message, CancellationTokenSource cts)
        {
            try
            {
                var text = Owner.Settings.MakeDeliverySafe && message.ParseMode is not null
                    ? MakeParseSafe(message.GetMessageText(), message.ParseMode.Value)
                    : message.GetMessageText();

                var resMessage = await Bot.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: mesId,
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