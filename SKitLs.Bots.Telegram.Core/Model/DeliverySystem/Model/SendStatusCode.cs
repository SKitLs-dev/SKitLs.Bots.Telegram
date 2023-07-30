namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model
{
    /// <summary>
    /// Represents server status code of delivery response.
    /// </summary>
    public enum SendStatusCode
    {
        OK = 200,

        MessageTypeNotDefined = 400,
        NoEditMessageId = 402,
        Forbidden = 403,
    }
}
