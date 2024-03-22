namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    /// <summary>
    /// Represents a base class for exceptions in the project.
    /// Contains information about the exception's origin and its description localization keys.
    /// </summary>
    public class SKTgException : Exception
    {
        /// <summary>
        /// Represents the origin of the thrown exception.
        /// </summary>
        public SKTEOriginType OriginType { get; private init; }

        /// <summary>
        /// Represents a specific prefix for localization keys.
        /// </summary>
        private const string LocalKeyPrefix = "exception";

        /// <summary>
        /// Represents the base key for resolving localization strings.
        /// </summary>
        public string KeyBase { get; private init; }

        /// <summary>
        /// Gets the localization key for accessing the localized exception caption.
        /// </summary>
        public string CaptionLocalKey => $"{LocalKeyPrefix}Cap.{KeyBase}";

        /// <summary>
        /// Gets the localization key for accessing the localized exception message.
        /// </summary>
        public string MessageLocalKey => $"{LocalKeyPrefix}Mes.{KeyBase}";

        /// <summary>
        /// Carries additional strings that should be written in the localized exception message.
        /// </summary>
        public string?[] Format { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SKTgException"/> class with specified data.
        /// </summary>
        /// <param name="localKey">The base key for resolving localization strings.</param>
        /// <param name="originType">The origin of the thrown exception.</param>
        /// <param name="format">Additional strings that should be written in the localized exception message.</param>
        public SKTgException(string localKey, SKTEOriginType originType, params string?[] format)
        {
            OriginType = originType;
            KeyBase = localKey;
            Format = format;
        }
    }
}