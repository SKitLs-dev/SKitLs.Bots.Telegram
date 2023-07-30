namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// Represents a general instance of bot user. Can be extended with additional functional.
    /// </summary>
    public interface IBotUser
    {
        /// <summary>
        /// User's telegram id. Can be used for sending messages instead of chat's id.
        /// </summary>
        public long TelegramId { get; }
    }
}