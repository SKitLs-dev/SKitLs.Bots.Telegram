namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    // XML-Doc Update
    /// <summary>
    /// An exception that represents an internal <see cref="SKTgException"/> exception.
    /// </summary>
    public class BotManagerException : SKTgSignedException
    {
        /// <summary>
        /// Creates a new instance of <see cref="BotManagerException"/> with specified data.
        /// </summary>
        /// <param name="localKey">Key base for resolving localization string.</param>
        /// <param name="sender">The object that has thrown exception.</param>
        /// <param name="format">Additional strings that should be written in localized exception message.</param>
        public BotManagerException(string localKey, object sender, params string?[] format)
            : base(localKey, SKTEOriginType.Internal, sender, format) { }
    }
}