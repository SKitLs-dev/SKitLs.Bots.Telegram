using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TEnums = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelievery
{
    public class AdvancedDelieverySystem : IDelieveryService
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;
        private ITelegramBotClient Bot => Owner.Bot;

        public bool IsParseSafe(ParseMode mode, string part) => mode switch
        {
            TEnums.ParseMode.Markdown => IsMarkdownSafe(part),
            _ => true,
        };
        public string MakeParseSafe(ParseMode mode, string part) => mode switch
        {
            TEnums.ParseMode.Markdown => MakeMarkdownSafe(part),
            _ => part,
        };

        private bool IsMarkdownSafe(string part)
        {
            int italic = 0;
            int bold = 0;
            int stroke = 0;
            foreach (char c in part)
            {
                if (c == '*') bold++;
                else if (c == '_') italic++;
                else if (c == '~') stroke++;
            }
            return italic % 2 == 0 && bold % 2 == 0 && stroke % 2 == 0;
        }
        private string MakeMarkdownSafe(string part)
        {
            string res = "";
            foreach (char c in part)
                if (c != '*' && c != '~' && c != '_') res += c;
            return res;
        }

        public async Task<DelieveryResponse> ReplyToSender(string message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(message, update.Sender.TelegramId, cts);
        public async Task<DelieveryResponse> ReplyToSender(IBuildableMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(message, update.Sender.TelegramId, cts);
        public async Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts = null)
        => await SendMessageToChatAsync(new BuildableMessage(message), chatId, cts);
        public async Task<DelieveryResponse> SendMessageToChatAsync(IBuildableMessage message, long chatId, CancellationTokenSource? cts = null)
        {
            cts ??= new();
            if (message is IEditWrapper edit) return await HandleEditAsync(edit, chatId, cts);
            else if (message is IOutputMessage output) return await SendOutputAsync(output, chatId, cts);
            else return await SendBuildableAsync(message, chatId, cts);
        }

        private async Task<DelieveryResponse> SendBuildableAsync(IBuildableMessage message, long chatId, CancellationTokenSource cts)
        {
            // TODO
            if (false) // Should be format && not format
            {
                Owner.LocalLogger.Warn("");
            }
            try
            {
                await Bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: message.GetMessageText(),
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cts.Token);
                return DelieveryResponse.OK();
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DelieveryResponse.Forbidden(e);
            }
        }
        private async Task<DelieveryResponse> SendOutputAsync(IOutputMessage message, long chatId, CancellationTokenSource cts)
        {
            try
            {
                await Bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: message.GetMessageText(),
                    parseMode: message.ParseMode,
                    // Entity   entites
                    // bool     disableWebPagePreview
                    // bool     disableNotification
                    // bool     protectContent
                    // [ long     replyToMessageId
                    // bool     allowSendingWithoutReply ]
                    replyMarkup: message.Menu?.GetMarkup(),
                    cancellationToken: cts.Token);
                return DelieveryResponse.OK();
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DelieveryResponse.Forbidden(e);
            }
        }

        private async Task<DelieveryResponse> HandleEditAsync(IEditWrapper message, long chatId, CancellationTokenSource cts)
        {
            if (message.Content is IOutputMessage output) return await EditOutputAsync(output, chatId, message.EditMessageId, cts);
            else return await EditBuildableAsync(message.Content, chatId, message.EditMessageId, cts);
        }
        private async Task<DelieveryResponse> EditBuildableAsync(IBuildableMessage message, long chatId, int mesId, CancellationTokenSource cts)
        {
            try
            {
                await Bot.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: mesId,
                    text: message.GetMessageText(),
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cts.Token);
                return DelieveryResponse.OK();
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DelieveryResponse.Forbidden(e);
            }
        }
        private async Task<DelieveryResponse> EditOutputAsync(IOutputMessage message, long chatId, int mesId, CancellationTokenSource cts)
        {
            // TODO
            if (message.Menu is not InlineKeyboardMarkup inline) throw new NotImplementedException();
            try
            {
                await Bot.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: mesId,
                    text: message.GetMessageText(),
                    parseMode: message.ParseMode,
                    replyMarkup: inline,
                    cancellationToken: cts.Token);
                return DelieveryResponse.OK();
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DelieveryResponse.Forbidden(e);
            }
        }
    }
}
