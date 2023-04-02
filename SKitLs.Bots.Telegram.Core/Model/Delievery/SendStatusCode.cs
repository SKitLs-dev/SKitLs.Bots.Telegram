namespace SKitLs.Bots.Telegram.Core.Model.Delievery
{
    public enum SendStatusCode
    {
        OK = 200,

        MessageTypeNotDefined = 400,
        NoEditMessageId = 402,
        Forbidden = 403,
    }
}
