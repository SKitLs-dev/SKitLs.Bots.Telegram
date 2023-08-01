using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem
{
    /// <summary>
    /// Default realization of <see cref="IDeliveryService"/> that works with string and simple <see cref="IBuildableMessage"/> messages.
    /// </summary>
    public class DefaultDeliveryService : IDeliveryService
    {
        private BotManager? _owner;
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        /// <summary>
        /// Specified method that raised during reflective <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/> compilation.
        /// Declare it to extend preset functionality.
        /// Invoked after <see cref="Owner"/> updating, but before recursive update.
        /// </summary>
        public Action<object, BotManager>? OnCompilation => null;
        
        /// <summary>
        /// A shortcut for owner's bot client.
        /// </summary>
        private ITelegramBotClient Bot => Owner.Bot;

        /// <summary>
        /// Checks if <paramref name="text"/> string is valid in a certain <paramref name="parseMode"/>.
        /// </summary>
        /// <param name="parseMode">Parsing mode of checker.</param>
        /// <param name="text">Text to be checked.</param>
        /// <returns><see langword="true"/> if is valid; otherwise <see langword="false"/>.</returns>
        public bool IsParseSafe(ParseMode parseMode, string text) => parseMode switch
        {
            ParseMode.Markdown => IsMarkdownSafe(text),
            _ => true,
        };

        /// <summary>
        /// Updates <paramref name="text"/> to be valid in a certain <paramref name="parseMode"/>.
        /// </summary>
        /// <param name="parseMode">Parsing mode of checker.</param>
        /// <param name="text">Text to be checked.</param>
        /// <returns>Safe in <paramref name="parseMode"/> text.</returns>
        public string MakeParseSafe(ParseMode parseMode, string text) => parseMode switch
        {
            ParseMode.Markdown => MakeMarkdownSafe(text),
            _ => text,
        };

        /// <summary>
        /// Checks if <paramref name="text"/> string is valid in <see cref="ParseMode.Markdown"/>.
        /// </summary>
        /// <param name="text">Text to be checked.</param>
        /// <returns><see langword="true"/> if markup is valid; otherwise <see langword="false"/>.</returns>
        private static bool IsMarkdownSafe(string text)
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
        /// <param name="text">Text to be checked.</param>
        /// <returns>Safe in <see cref="ParseMode.Markdown"/> text.</returns>
        private static string MakeMarkdownSafe(string text)
        {
            if (IsMarkdownSafe(text)) return text;
            string res = string.Empty;
            foreach (char c in text)
                if (c != '*' && c != '~' && c != '_')
                    res += c;
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
    }
}