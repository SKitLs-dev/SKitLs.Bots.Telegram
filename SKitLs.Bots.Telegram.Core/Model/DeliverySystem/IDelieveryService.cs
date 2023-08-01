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
    public interface IDeliveryService : IOwnerCompilable
    {
        /// <summary>
        /// Checks if <paramref name="text"/> string is valid in a certain <paramref name="parseMode"/>.
        /// </summary>
        /// <param name="parseMode">Parsing mode of checker.</param>
        /// <param name="text">Text to be checked.</param>
        /// <returns><see langword="true"/> if is valid; otherwise <see langword="false"/>.</returns>
        public bool IsParseSafe(ParseMode parseMode, string text);
        /// <summary>
        /// Checks and updates <paramref name="text"/> to be valid in a certain <paramref name="parseMode"/>.
        /// </summary>
        /// <param name="parseMode">Parsing mode of checker.</param>
        /// <param name="text">Text to be checked.</param>
        /// <returns>Safe in <paramref name="parseMode"/> text.</returns>
        public string MakeParseSafe(ParseMode parseMode, string text);

        /// <summary>
        /// Asynchronously sends string message to a certain chat by its id.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="chatId">Recipient's chat ID.</param>
        /// <param name="cts">Cancellation token source.</param>
        public Task<DeliveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts = null);
        
        /// <summary>
        /// Asynchronously sends <see cref="IBuildableMessage"/> to a certain chat by its id.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="chatId">Recipient's chat ID.</param>
        /// <param name="cts">Cancellation token source.</param>
        public Task<DeliveryResponse> SendMessageToChatAsync(IBuildableMessage message, long chatId, CancellationTokenSource? cts = null);

        /// <summary>
        /// Asynchronously sends string message to a certain chat by the update
        /// it has raised
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="update">An incoming update.</param>
        /// <param name="cts">Cancellation token source.</param>
        public Task<DeliveryResponse> ReplyToSender(string message, ISignedUpdate update, CancellationTokenSource? cts = null);
        
        /// <summary>
        /// Asynchronously sends <see cref="IBuildableMessage"/> to a certain chat by the update
        /// it has raised.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="update">An incoming update.</param>
        /// <param name="cts">Cancellation token source.</param>
        public Task<DeliveryResponse> ReplyToSender(IBuildableMessage message, ISignedUpdate update, CancellationTokenSource? cts = null);
    }
}