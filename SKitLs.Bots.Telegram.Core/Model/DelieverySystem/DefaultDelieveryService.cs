using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TEnums = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem
{
    public class DefaultDelieveryService : IDelieveryService
    {
        public BotManager Owner { get; set; }
        private ITelegramBotClient Bot => Owner.Bot;

        public DefaultDelieveryService(BotManager owner)
        {
            Owner = owner;
        }
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
                if (c == '*')
                    bold++;
                else if (c == '_')
                    italic++;
                else if (c == '~')
                    stroke++;
            }
            return italic % 2 == 0 && bold % 2 == 0 && stroke % 2 == 0;
        }
        private string MakeMarkdownSafe(string part)
        {
            string res = "";
            foreach (char c in part)
                if (c != '*' && c != '~' && c != '_')
                    res += c;
            return res;
        }


        public async Task<DelieveryResponse> SendMessageToChatAsync(IOutputMessage message, long chatId, CancellationTokenSource? cts = null)
        {
            cts ??= new();
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
                    replyMarkup: message.Markup,
                    cancellationToken: cts.Token);
                return DelieveryResponse.OK();
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DelieveryResponse.Forbidden(e);
            }
        }

        public async Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts = null)
        {
            cts ??= new();
            try
            {
                await Bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: message,
                    parseMode: ParseMode.Markdown,
                    // Entity   entites
                    // bool     disableWebPagePreview
                    // bool     disableNotification
                    // bool     protectContent
                    // [ long     replyToMessageId
                    // bool     allowSendingWithoutReply ]
                    cancellationToken: cts.Token);
                return DelieveryResponse.OK();
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DelieveryResponse.Forbidden(e);
            }
        }

        public Task<DelieveryResponse> ReplyToSender(string message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => SendMessageToChatAsync(message, update.Sender.TelegramId, cts);

        public Task<DelieveryResponse> ReplyToSender(IOutputMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => SendMessageToChatAsync(message, update.Sender.TelegramId, cts);
    }
}