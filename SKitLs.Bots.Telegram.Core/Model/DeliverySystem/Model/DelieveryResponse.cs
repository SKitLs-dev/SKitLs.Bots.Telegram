using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model
{
    /// <summary>
    /// Represents a response of a delivery service <see cref="IDeliveryService"/>.
    /// If not OK, contains information about raised exception.
    /// </summary>
    [Obsolete("TODO. Will be rebuilt in future versions.")]
    public class DeliveryResponse
    {
        /// <summary>
        /// Server status code.
        /// </summary>
        public SendStatusCode StatusCode { get; private set; }
        /// <summary>
        /// Represents either message was successfully sent.
        /// </summary>
        public bool Success => StatusCode == SendStatusCode.OK;
        /// <summary>
        /// Response message.
        /// </summary>
        public string ResponseMessage { get; private set; }
        /// <summary>
        /// Response exception.
        /// </summary>
        public Exception? Exception { get; private set; }
        /// <summary>
        /// Message that has been sent.
        /// </summary>
        public Message? Message { get; private set; }
        
        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/> with specific data.
        /// </summary>
        /// <param name="responseMessage">Response message (OK or short info).</param>
        /// <param name="status">Response status.</param>
        private DeliveryResponse(string responseMessage, SendStatusCode status)
        {
            ResponseMessage = responseMessage;
            StatusCode = status;
        }
        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/> with specific data.
        /// </summary>
        /// <param name="responseMessage">Response message (OK or short info).</param>
        /// <param name="status">Response status.</param>
        /// <param name="message">Message that has been sent.</param>
        private DeliveryResponse(string responseMessage, SendStatusCode status, Message? message = null)
            : this(responseMessage, status) => Message = message;
        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/> with specific data.
        /// </summary>
        /// <param name="responseMessage">Response message (OK or short info).</param>
        /// <param name="status">Response status.</param>
        /// <param name="e">Exception that has been thrown.</param>
        private DeliveryResponse(string responseMessage, SendStatusCode status, Exception? e = null)
            : this(responseMessage, status) => Exception = e;

        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/> with <see cref="SendStatusCode.OK"/>.
        /// </summary>
        public static DeliveryResponse OK(Message message) => new("OK", SendStatusCode.OK, message);
        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/>
        /// with <see cref="SendStatusCode.Forbidden"/> and custom exception.
        /// </summary>
        public static DeliveryResponse Forbidden(Exception e)
            => new("Пользователь запретил боту писать ему", SendStatusCode.Forbidden, e);
        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/>
        /// with <see cref="SendStatusCode.NoEditMessageId"/>.
        /// </summary>
        public static DeliveryResponse NoEditMessageId()
            => new("ID сообщения для редактирования не было определено", SendStatusCode.NoEditMessageId);
        /// <summary>
        /// Creates a new instance of <see cref="DeliveryResponse"/>
        /// with <see cref="SendStatusCode.MessageTypeNotDefined"/>.
        /// </summary>
        public static DeliveryResponse UnknownMessageType()
            => new("Тип сообщения для отправки не определён", SendStatusCode.MessageTypeNotDefined);
    }
}