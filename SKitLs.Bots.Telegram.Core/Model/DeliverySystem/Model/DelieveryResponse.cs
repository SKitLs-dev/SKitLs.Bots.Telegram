using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model
{
    /// <summary>
    /// Represents a response of a delievery service <see cref="IDelieveryService"/>.
    /// If not OK, contains information about raised exception.
    /// </summary>
    [Obsolete("TODO. Will be rebuilt in future versions.")]
    public class DelieveryResponse
    {
        /// <summary>
        /// Server status code.
        /// </summary>
        public SendStatusCode StatusCode { get; private set; }
        /// <summary>
        /// Represents either message was successfuly sent.
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
        /// Creates a new instance of <see cref="DelieveryResponse"/> with specific data.
        /// </summary>
        /// <param name="responseMessage">Response message (OK or short info)</param>
        /// <param name="status">Response status</param>
        private DelieveryResponse(string responseMessage, SendStatusCode status)
        {
            ResponseMessage = responseMessage;
            StatusCode = status;
        }
        /// <summary>
        /// Creates a new instance of <see cref="DelieveryResponse"/> with specific data.
        /// </summary>
        /// <param name="responseMessage">Response message (OK or short info)</param>
        /// <param name="status">Response status</param>
        /// <param name="message">Message that has been sent</param>
        private DelieveryResponse(string responseMessage, SendStatusCode status, Message? message = null)
            : this(responseMessage, status) => Message = message;
        /// <summary>
        /// Creates a new instance of <see cref="DelieveryResponse"/> with specific data.
        /// </summary>
        /// <param name="responseMessage">Response message (OK or short info)</param>
        /// <param name="status">Response status</param>
        /// <param name="e">Exception that has been thrown</param>
        private DelieveryResponse(string responseMessage, SendStatusCode status, Exception? e = null)
            : this(responseMessage, status) => Exception = e;

        /// <summary>
        /// Creates a new instance of <see cref="DelieveryResponse"/> with <see cref="SendStatusCode.OK"/>.
        /// </summary>
        public static DelieveryResponse OK(Message message) => new("OK", SendStatusCode.OK, message);
        /// <summary>
        /// Creates a new instance of <see cref="DelieveryResponse"/>
        /// with <see cref="SendStatusCode.Forbidden"/> and custom exception.
        /// </summary>
        public static DelieveryResponse Forbidden(Exception e)
            => new("Пользователь запретил боту писать ему", SendStatusCode.Forbidden, e);
        /// <summary>
        /// Creates a new instance of <see cref="DelieveryResponse"/>
        /// with <see cref="SendStatusCode.NoEditMessageId"/>.
        /// </summary>
        public static DelieveryResponse NoEditMessageId()
            => new("ID сообщения для редактирования не было определено", SendStatusCode.NoEditMessageId);
        /// <summary>
        /// Creates a new instance of <see cref="DelieveryResponse"/>
        /// with <see cref="SendStatusCode.MessageTypeNotDefined"/>.
        /// </summary>
        public static DelieveryResponse UnknownMessageType()
            => new("Тип сообщения для отправки не определён", SendStatusCode.MessageTypeNotDefined);
    }
}