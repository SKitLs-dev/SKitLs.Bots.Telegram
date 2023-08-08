using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model
{
    // XML-Doc Update
    /// <summary>
    /// Default realization of <see cref="IBotUser"/> interface. Provides basics for proper update handling.
    /// </summary>
    public class DefaultBotUser : IBotUser
    {
        /// <summary>
        /// User's telegram id. Can be used for sending messages instead of chat's id.
        /// </summary>
        public long TelegramId { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DefaultBotUser"/> with specified data.
        /// </summary>
        /// <param name="id">User's telegram id.</param>
        public DefaultBotUser(long id) => TelegramId = id;
    }
}