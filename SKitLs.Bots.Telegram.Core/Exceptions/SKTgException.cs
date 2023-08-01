namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    /// <summary>
    /// Represents a base for project's exceptions. Contains information about exception's origin and its description
    /// localization keys.
    /// </summary>
    public class SKTgException : Exception
    {
        /// <summary>
        /// Represents the origin of thrown exception.
        /// </summary>
        public SKTEOriginType OriginType { get; private init; }

        /// <summary>
        /// Represents specific prefix for localization keys.
        /// </summary>
        private const string LocalKeyPrefix = "exception";
        /// <summary>
        /// Represents a key base for resolving localization string.
        /// </summary>
        public string KeyBase { get; private init; }
        /// <summary>
        /// Localization key, by which localized exception caption could be accessed.
        /// </summary>
        public string CaptionLocalKey => $"{LocalKeyPrefix}Cap.{KeyBase}";
        /// <summary>
        /// Localization key, by which localized exception message could be accessed.
        /// </summary>
        public string MessageLocalKey => $"{LocalKeyPrefix}Mes.{KeyBase}";

        /// <summary>
        /// Carries additional strings that should be written in localized exception message.
        /// </summary>
        public string?[] Format { get; private init; }

        /// <summary>
        /// Creates a new instance of <see cref="SKTgException"/> with specified data.
        /// </summary>
        /// <param name="localKey">Key base for resolving localization string.</param>
        /// <param name="originType">The origin of thrown exception.</param>
        /// <param name="format">Additional strings that should be written in localized exception message.</param>
        public SKTgException(string localKey, SKTEOriginType originType, params string?[] format)
        {
            OriginType = originType;
            KeyBase = localKey;
            Format = format;
        }
    }
}