using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.Stateful.Exceptions.External
{
    /// <summary>
    /// Represents an exception that occurs when an object is not a stateful user and an operation requiring
    /// stateful user functionality is attempted.
    /// </summary>
    public class NotStatefulException : StatefulException
    {
        /// <summary>
        /// Creates a new instance of <see cref="NotStatefulException"/> with the specified sender.
        /// </summary>
        /// <param name="sender">The object that triggered the exception.</param>
        public NotStatefulException(object sender) : base("NotStatefulUser", SKTEOriginType.Inexternal, sender)
        { }
    }
}