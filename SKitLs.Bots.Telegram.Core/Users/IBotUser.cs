namespace SKitLs.Bots.Telegram.Core.Model.Users
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

        /// <summary>
        /// Gets a value indicating whether the user is a Telegram Premium user.
        /// </summary>
        public bool IsPremium { get; }

        /// <summary>
        /// Gets the language code of the user. Used <see href="https://en.wikipedia.org/wiki/IETF_language_tag">IETF language tag</see>.
        /// </summary>
        public string LanguageCode { get; }

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        public string? Username { get; }

        /// <summary>
        /// Gets the first name of the user.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name of the user.
        /// </summary>
        public string? LastName { get; }
    }
}