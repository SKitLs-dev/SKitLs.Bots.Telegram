namespace SKitLs.Bots.Telegram.Core.Model.Users
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IBotUser"/> interface, providing basics for proper update handling.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DefaultBotUser"/> class with the specified Telegram ID, premium status, language code, and first name.
    /// </remarks>
    /// <param name="id">The user's Telegram ID.</param>
    /// <param name="isPremium">A boolean indicating whether the user is a premium user.</param>
    /// <param name="languageCode">The language code of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    public class DefaultBotUser(long id, bool isPremium, string languageCode, string firstName) : IBotUser
    {
        /// <inheritdoc/>
        public long TelegramId { get; set; } = id;

        /// <inheritdoc/>
        public bool IsPremium { get; set; } = isPremium;

        /// <inheritdoc/>
        public string LanguageCode { get; set; } = languageCode;

        /// <inheritdoc/>
        public string? Username { get; set; }

        /// <inheritdoc/>
        public string FirstName { get; set; } = firstName;

        /// <inheritdoc/>
        public string? LastName { get; set; }
    }
}