namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    /// <summary>
    /// An enum that describes the origin of thrown <see cref="SKTgException"/>.
    /// </summary>
    public enum SKTEOriginType
    {
        /// <summary>
        /// Represents an internal exception, which means that exception was thrown as a result of some internal processes.
        /// For example, some data was not defined.
        /// </summary>
        Internal = -10,
        /// <summary>
        /// Represents an exception, that could be thrown either by some internal processes or external actions.
        /// For example, some property was not defined or was overridden incorrectly.
        /// </summary>
        Inexternal = 0,
        /// <summary>
        /// Represents an external exception, which means that exception was thrown as a result of some external actions.
        /// For example, user tried to add somewhat duplicate.
        /// </summary>
        External = 10,
    }

    /// <summary>
    /// Represents a base for project's exceptions. Contains information about exception's origin and its description
    /// localization keys.
    /// </summary>
    public class SKTgException : Exception
    {
        /// <summary>
        /// Represents the origin of thrown exception.
        /// </summary>
        public SKTEOriginType OriginType { get; private set; }

        /// <summary>
        /// Represents specific prefix for localization keys.
        /// </summary>
        private const string LocalKeyPrefix = "exception";
        /// <summary>
        /// Represents a key base for resolving localization string.
        /// </summary>
        public string KeyBase { get; private set; }
        /// <summary>
        /// Localization key, by which localized exception caption could be accessed.
        /// </summary>
        public string CaptionLocalKey => $"{LocalKeyPrefix}Cap.{KeyBase}";
        /// <summary>
        /// Localization key, by which localized exception message could be accessed.
        /// </summary>
        public string MessgeLocalKey => $"{LocalKeyPrefix}Mes.{KeyBase}";

        /// <summary>
        /// Carries additional strings that should be written in localized exception message.
        /// </summary>
        public string?[] Format { get; private set; }

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