namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// Default realization of <see cref="IBotUser"/> interface. Provides basics for proper update handling.
    /// </summary>
    public class DefaultBotUser : IBotUser
    {
        public long TelegramId { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DefaultBotUser"/> with specified data.
        /// </summary>
        /// <param name="id">User's telegram id.</param>
        public DefaultBotUser(long id) => TelegramId = id;
    }
}