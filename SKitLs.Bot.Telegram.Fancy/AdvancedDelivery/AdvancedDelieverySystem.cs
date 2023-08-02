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
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <summary>
        /// Specified method that raised during reflective <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/> compilation.
        /// Declare it to extend preset functionality.
        /// Invoked after <see cref="Owner"/> updating, but before recursive update.
        /// </summary>
        public Action<object, BotManager>? OnCompilation => null;
        private ITelegramBotClient Bot => Owner.Bot;

        /// <summary>
        /// Checks if <paramref name="text"/> string is valid in a certain <paramref name="parseMode"/>.
        /// </summary>
        /// <param name="parseMode">Parsing mode of checker.</param>
        /// <param name="text">Text to be checked.</param>
        /// <returns><see langword="true"/> if is valid; otherwise <see langword="false"/>.</returns>
        public bool IsParseSafe(ParseMode parseMode, string text) => parseMode switch
        {
            TEnums.ParseMode.Markdown => IsMarkdownSafe(text),
            _ => true,
        };
        /// <summary>
        /// Checks and updates <paramref name="text"/> to be valid in a certain <paramref name="parseMode"/>.
        /// </summary>
        /// <param name="parseMode">Parsing mode of checker.</param>
        /// <param name="text">Text to be checked.</param>
        /// <returns>Safe in <paramref name="parseMode"/> text.</returns>
        public string MakeParseSafe(ParseMode parseMode, string text) => parseMode switch
        {
            TEnums.ParseMode.Markdown => MakeMarkdownSafe(text),
            _ => text,
        };

        private bool IsMarkdownSafe(string text)
        {
            int italic = 0;
            int bold = 0;
            int stroke = 0;
            foreach (char c in text)
            {
                if (c == '*') bold++;
                else if (c == '_') italic++;
                else if (c == '~') stroke++;
            }
            return italic % 2 == 0 && bold % 2 == 0 && stroke % 2 == 0;
        }
        private string MakeMarkdownSafe(string text)
        {
            string res = "";
            foreach (char c in text)
                if (c != '*' && c != '~' && c != '_') res += c;
            return res;
        }

        /// <summary>
        /// Asynchronously sends string message to a certain chat by the update
        /// it has raised
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="update">An incoming update.</param>
        /// <param name="cts">Cancellation token source.</param>
        public async Task<DeliveryResponse> ReplyToSender(string message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(message, update.Sender.TelegramId, cts);
        /// <summary>
        /// Asynchronously sends <see cref="IBuildableMessage"/> to a certain chat by the update
        /// it has raised.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="update">An incoming update.</param>
        /// <param name="cts">Cancellation token source.</param>
        public async Task<DeliveryResponse> ReplyToSender(IBuildableMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(message, update.Sender.TelegramId, cts);
        /// <summary>
        /// Asynchronously sends string message to a certain chat by its id.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="chatId">Recipient's chat ID.</param>
        /// <param name="cts">Cancellation token source.</param>
        public async Task<DeliveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts = null)
        => await SendMessageToChatAsync(new BuildableMessage(message), chatId, cts);
        /// <summary>
        /// Asynchronously sends <see cref="IBuildableMessage"/> to a certain chat by its id.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="chatId">Recipient's chat ID.</param>
        /// <param name="cts">Cancellation token source.</param>
        public async Task<DeliveryResponse> SendMessageToChatAsync(IBuildableMessage message, long chatId, CancellationTokenSource? cts = null)
        {
            cts ??= new();
            if (message is IEditWrapper edit) return await HandleEditAsync(edit, chatId, cts);
            else if (message is IOutputMessage output) return await SendOutputAsync(output, chatId, cts);
            else return await SendBuildableAsync(message, chatId, cts);
        }

        private async Task<DeliveryResponse> SendBuildableAsync(IBuildableMessage message, long chatId, CancellationTokenSource cts)
        {
            // TODO
            if (false) // Should be format && not format
            {
                Owner.LocalLogger.Warn("");
            }
            try
            {
                var res = await Bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: message.GetMessageText(),
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cts.Token);
                return DeliveryResponse.OK(res);
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DeliveryResponse.Forbidden(e);
            }
        }
        private async Task<DeliveryResponse> SendOutputAsync(IOutputMessage message, long chatId, CancellationTokenSource cts)
        {
            try
            {
                var res = await Bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: message.GetMessageText(),
                    parseMode: message.ParseMode,
                    // Entity   entities
                    // bool     disableWebPagePreview
                    // bool     disableNotification
                    // bool     protectContent
                    // [ long     replyToMessageId
                    // bool     allowSendingWithoutReply ]
                    replyMarkup: message.Menu?.GetMarkup(),
                    cancellationToken: cts.Token);
                return DeliveryResponse.OK(res);
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DeliveryResponse.Forbidden(e);
            }
        }

        private async Task<DeliveryResponse> HandleEditAsync(IEditWrapper message, long chatId, CancellationTokenSource cts)
        {
            if (message.Content is IOutputMessage output) return await EditOutputAsync(output, chatId, message.EditMessageId, cts);
            else return await EditBuildableAsync(message.Content, chatId, message.EditMessageId, cts);
        }
        private async Task<DeliveryResponse> EditBuildableAsync(IBuildableMessage message, long chatId, int mesId, CancellationTokenSource cts)
        {
            try
            {
                var res = await Bot.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: mesId,
                    text: message.GetMessageText(),
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cts.Token);
                return DeliveryResponse.OK(res);
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DeliveryResponse.Forbidden(e);
            }
        }
        private async Task<DeliveryResponse> EditOutputAsync(IOutputMessage message, long chatId, int mesId, CancellationTokenSource cts)
        {
            // TODO
            var menu = message.Menu?.GetMarkup() as InlineKeyboardMarkup;
            //if (menu is not null && menu is not InlineKeyboardMarkup inline)
            //    throw new NotImplementedException();
            try
            {
                var res = await Bot.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: mesId,
                    text: message.GetMessageText(),
                    parseMode: message.ParseMode,
                    replyMarkup: menu,
                    cancellationToken: cts.Token);
                return DeliveryResponse.OK(res);
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DeliveryResponse.Forbidden(e);
            }
        }
    }
}