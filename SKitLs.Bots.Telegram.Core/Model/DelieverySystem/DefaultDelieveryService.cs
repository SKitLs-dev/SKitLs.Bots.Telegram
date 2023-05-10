using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TEnums = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem
{
    public class DefaultDelieveryService : IDelieveryService
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

        public async Task<DelieveryResponse> ReplyToSender(string message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(message, update.Sender.TelegramId, cts);
        public async Task<DelieveryResponse> ReplyToSender(IBuildableMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(message, update.Sender.TelegramId, cts);
        public async Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(new BuildableMessage(message), chatId, cts);
        public async Task<DelieveryResponse> SendMessageToChatAsync(IBuildableMessage message, long chatId, CancellationTokenSource? cts = null)
        {
            cts ??= new();
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
    }
}