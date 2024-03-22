using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.Services;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem
{
    /// <summary>
    /// An interface that provides ways of sending messages to users.
    /// </summary>
    public partial interface IDeliveryService : IBotService
    {
        /// <summary>
        /// Asynchronously sends a string message to a certain chat by the update it has raised.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="update">An incoming update.</param>
        /// <param name="cts">The cancellation token source.</param>
        /// <returns>A task representing the asynchronous operation with a <see cref="DeliveryResponse"/>.</returns>
        public Task<DeliveryResponse> AnswerSenderAsync(string message, ISignedUpdate update, CancellationTokenSource? cts = null);

        /// <summary>
        /// Asynchronously sends an <see cref="ITelegramMessage"/> to a certain chat by the update it has raised.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="update">An incoming update.</param>
        /// <param name="cts">The cancellation token source.</param>
        /// <returns>A task representing the asynchronous operation with a <see cref="DeliveryResponse"/>.</returns>
        public Task<DeliveryResponse> AnswerSenderAsync(ITelegramMessage message, ISignedUpdate update, CancellationTokenSource? cts = null);

        /// <summary>
        /// Asynchronously sends a string message to a certain chat by its ID.
        /// </summary>
        /// <param name="chatId">The recipient's or chat's ID.</param>
        /// <param name="message">The message to be sent.</param>
        /// <param name="cts">The cancellation token source.</param>
        /// <returns>A task representing the asynchronous operation with a <see cref="DeliveryResponse"/>.</returns>
        public Task<DeliveryResponse> SendMessageToChatAsync(long chatId, string message, CancellationTokenSource? cts = null);

        /// <summary>
        /// Asynchronously sends an <see cref="ITelegramMessage"/> to a certain chat by its ID.
        /// </summary>
        /// <param name="chatId">The recipient's or chat's ID.</param>
        /// <param name="message">The message to be sent.</param>
        /// <param name="cts">The cancellation token source.</param>
        /// <returns>A task representing the asynchronous operation with a <see cref="DeliveryResponse"/>.</returns>
        public Task<DeliveryResponse> SendMessageToChatAsync(long chatId, ITelegramMessage message, CancellationTokenSource? cts = null);

        /// <summary>
        /// Checks if the <paramref name="text"/> string is valid in a certain <paramref name="parseMode"/>.
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <param name="parseMode">The parsing mode used for checking.</param>
        /// <returns><see langword="true"/> if the text is valid; otherwise, <see langword="false"/>.</returns>
        public bool IsParseSafe(string text, ParseMode parseMode) => parseMode switch
        {
            ParseMode.Markdown => IsMarkdownSafe(text),
            _ => true,
        };

        /// <summary>
        /// Checks and updates the <paramref name="text"/> to be valid in a certain <paramref name="parseMode"/>.
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <param name="parseMode">The parsing mode used for checking.</param>
        /// <returns>The text safe in the <paramref name="parseMode"/>.</returns>
        public string MakeParseSafe(string text, ParseMode parseMode) => parseMode switch
        {
            ParseMode.Markdown => MakeMarkdownSafe(text),
            _ => text,
        };

        /// <summary>
        /// Checks if the given string <paramref name="text"/> can be safely used as Markdown content.
        /// <para/>
        /// <i>Will be moved to stand-alone text helper.</i>
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <returns><see langword="true"/> if the text is safe for Markdown; otherwise, <see langword="false"/>.</returns>
        public static bool IsMarkdownSafe(string text)
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
        /// Makes the given string <paramref name="text"/> safe for use as Markdown content by escaping any Markdown-related characters.
        /// <para/>
        /// <i>Will be moved to stand-alone text helper.</i>
        /// </summary>
        /// <param name="text">The text to be made Markdown safe.</param>
        /// <returns>The Markdown-safe version of the input text.</returns>
        public static string MakeMarkdownSafe(string text)
        {
            if (IsMarkdownSafe(text)) return text;
            string res = string.Empty;
            foreach (char c in text)
                if (c != '*' && c != '~' && c != '_')
                    res += c;
            return res;
        }
    }
}