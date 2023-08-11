using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.Stateful.Exceptions.Inexternal
{
    /// <summary>
    /// An exception indicating that there is no default section available in a stateful context.
    /// </summary>
    public class NoDefaultSectionException : StatefulException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoDefaultSectionException"/> class.
        /// </summary>
        /// <param name="sender">The object that triggered the exception.</param>
        public NoDefaultSectionException(object sender) : base("NoDefaultSection", SKTEOriginType.Inexternal, sender)
        { }
    }
}