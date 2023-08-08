using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem
{
    /// <summary>
    /// Represents a response of a delivery service implemented by <see cref="IDeliveryService"/>.
    /// <para/>
    /// If the delivery is successful, contains an instance of the sent message.
    /// Otherwise contains information about the raised exception.
    /// </summary>
    public sealed class DeliveryResponse
    {
        /// <summary>
        /// Gets a value indicating whether the delivery was successful.
        /// </summary>
        public bool Success => _message is not null;

        private Message? _message;
        /// <summary>
        /// Represents an instance of the message in case it has been successfully sent.
        /// </summary>
        /// <exception cref="NullReferenceException"/>
        public Message SentMessage
        {
            get => _message ?? throw new NullReferenceException();
            private init => _message = value;
        }

        private Exception? _exception;
        /// <summary>
        /// <i>Optional.</i> Represents the exception that occurred during the delivery.
        /// </summary>
        /// <exception cref="NullReferenceException"/>
        public Exception Exception
        {
            get => _exception ?? throw new NullReferenceException();
            private init => _exception = value;
        }
        /// <summary>
        /// Gets the <see cref="ApiRequestException"/>, if applicable.
        /// </summary>
        public ApiRequestException? ApiException => _exception as ApiRequestException;

        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/> with specific data representing a successful delivery.
        /// </summary>
        /// <param name="message">The message that has been sent.</param>
        public DeliveryResponse(Message message) => SentMessage = message;

        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/> with specific data representing a failed delivery.
        /// </summary>
        /// <param name="exception">The exception that occurred during the delivery.</param>
        public DeliveryResponse(Exception exception) => Exception = exception;
    }
}