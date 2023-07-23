namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    /// <summary>
    /// An exception that represents an internal <see cref="SKTgException"/> exception.
    /// </summary>
    public class BotManagerExcpetion : SKTgException
    {
        /// <summary>
        /// Creates a new instance of <see cref="BotManagerExcpetion"/> with specified data.
        /// </summary>
        /// <param name="localKey">Key base for resolving localization string.</param>
        /// <param name="format">Additional strings that should be written in localized exception message.</param>
        public BotManagerExcpetion(string localKey, params string?[] format)
            : base(localKey, SKTEOriginType.Internal, format) { }
    }
}