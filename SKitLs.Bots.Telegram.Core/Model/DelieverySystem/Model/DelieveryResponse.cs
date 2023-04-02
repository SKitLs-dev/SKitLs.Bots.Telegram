namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model
{
    public class DelieveryResponse
    {
        public SendStatusCode StatusCode { get; set; }
        public bool Success => StatusCode == SendStatusCode.OK;
        public string Message { get; set; }

        private DelieveryResponse(string message)
        {
            Message = message;
            StatusCode = SendStatusCode.OK;
        }
        private DelieveryResponse(string message, SendStatusCode status)
        {
            Message = message;
            StatusCode = status;
        }

        public static DelieveryResponse OK() => new("OK");
        public static DelieveryResponse Forbidden()
            => new("Пользователь запретил боту писать ему", SendStatusCode.Forbidden);
        public static DelieveryResponse NoEditMessageId()
            => new("ID сообщения для редактирования не было определено", SendStatusCode.NoEditMessageId);
        public static DelieveryResponse UnknownMessageType()
            => new("Тип сообщения для отправки не определён", SendStatusCode.MessageTypeNotDefined);
    }
}
