namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    /// <summary>
    /// An exception that represents an internal <see cref="SKTgException"/> exception.
    /// Base class for all internal errors.
    /// </summary>
    public class BotManagerException : SKTgSignedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BotManagerException"/> class with specified data.
        /// </summary>
        /// <param name="localKey">Key base for resolving localization string.</param>
        /// <param name="sender">The object that has thrown the exception.</param>
        /// <param name="format">Additional strings that should be written in localized exception message.</param>
        public BotManagerException(string localKey, object sender, params string?[] format)
            : base(localKey, SKTEOriginType.Internal, sender, format) { }
    }
}