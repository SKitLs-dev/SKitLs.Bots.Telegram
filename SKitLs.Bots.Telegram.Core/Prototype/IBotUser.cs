namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// Represents a fundamental bot user instance abstraction that can be extended with additional functionality.
    /// </summary>
    public interface IBotUser
    {
        /// <summary>
        /// Gets the Telegram ID of the user.
        /// <para/>
        /// Can be used for sending messages instead of the chat's ID.
        /// </summary>
        public long TelegramId { get; }
    }
}