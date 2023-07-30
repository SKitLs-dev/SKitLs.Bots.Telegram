using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem
{
    /// <summary>
    /// An interface that provides ways of sending messages to users.
    /// </summary>
    public interface IDelieveryService : IOwnerCompilable
    {
        /// <summary>
        /// Checks if <paramref name="text"/> string is valid in a certain <paramref name="parsemode"/>.
        /// </summary>
        /// <param name="parsemode">Parsing mode of chcker</param>
        /// <param name="text">Text to be checked</param>
        /// <returns><see langword="true"/> if is valid; otherwise <see langword="false"/>.</returns>
        public bool IsParseSafe(ParseMode parsemode, string text);
        /// <summary>
        /// Checks and updates <paramref name="text"/> to be valid in a certain <paramref name="parsemode"/>.
        /// </summary>
        /// <param name="parsemode">Parsing mode of chcker</param>
        /// <param name="text">Text to be checked</param>
        /// <returns>Safe in <paramref name="parsemode"/> text.</returns>
        public string MakeParseSafe(ParseMode parsemode, string text);

        /// <summary>
        /// Asynchronously sends string message to a certain chat by its id.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="chatId">Recipient's chat ID</param>
        /// <param name="cts">Cancellation token source</param>
        public Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts = null);
        /// <summary>
        /// Asynchronously sends <see cref="IBuildableMessage"/> to a certain chat by its id.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="chatId">Recipient's chat ID</param>
        /// <param name="cts">Cancellation token source</param>
        public Task<DelieveryResponse> SendMessageToChatAsync(IBuildableMessage message, long chatId, CancellationTokenSource? cts = null);

        /// <summary>
        /// Asynchronously sends string message to a certain chat by the update
        /// it has raised
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="update">Recieved update</param>
        /// <param name="cts">Cancellation token source</param>
        public Task<DelieveryResponse> ReplyToSender(string message, ISignedUpdate update, CancellationTokenSource? cts = null);
        /// <summary>
        /// Asynchronously sends <see cref="IBuildableMessage"/> to a certain chat by the update
        /// it has raised.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="update">Recieved update</param>
        /// <param name="cts">Cancellation token source</param>
        public Task<DelieveryResponse> ReplyToSender(IBuildableMessage message, ISignedUpdate update, CancellationTokenSource? cts = null);
    }
}