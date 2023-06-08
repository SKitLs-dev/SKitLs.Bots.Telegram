using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem
{
    /// <summary>
    /// Default realization of <see cref="IDelieveryService"/> that works with string
    /// and simiple <see cref="IBuildableMessage"/> messages.
    /// </summary>
    public class DefaultDelieveryService : IDelieveryService
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;
        
        /// <summary>
        /// A shortcut for owner's bot client.
        /// </summary>
        private ITelegramBotClient Bot => Owner.Bot;

        public bool IsParseSafe(ParseMode parsemode, string text) => parsemode switch
        {
            ParseMode.Markdown => IsMarkdownSafe(text),
            _ => true,
        };
        public string MakeParseSafe(ParseMode parsemode, string text) => parsemode switch
        {
            ParseMode.Markdown => MakeMarkdownSafe(text),
            _ => text,
        };

        /// <summary>
        /// Checks if <paramref name="text"/> string is valid in <see cref="ParseMode.Markdown"/>.
        /// </summary>
        /// <param name="text">Text to be checked</param>
        /// <returns><see langword="true"/> if markup is valid; otherwise <see langword="false"/>.</returns>
        private bool IsMarkdownSafe(string text)
        {
            int italic = 0;
            int bold = 0;
            int stroke = 0;
            foreach (char c in text)
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
        /// <summary>
        /// Checks and updates <paramref name="text"/> to be valid in <see cref="ParseMode.Markdown"/>.
        /// </summary>
        /// <param name="text">Text to be checked</param>
        /// <returns>Safe in <see cref="ParseMode.Markdown"/> text.</returns>
        private string MakeMarkdownSafe(string text)
        {
            if (IsMarkdownSafe(text)) return text;
            string res = string.Empty;
            foreach (char c in text)
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
                var res = await Bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: message.GetMessageText(),
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cts.Token);
                return DelieveryResponse.OK(res);
            }
            catch (Exception e)
            {
                cts.Cancel();
                return DelieveryResponse.Forbidden(e);
            }
        }
    }
}