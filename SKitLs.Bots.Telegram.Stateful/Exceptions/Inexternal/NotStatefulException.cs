using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.Stateful.Exceptions.Inexternal
{
    /// <summary>
    /// Represents an exception that occurs when an object does not have stateful user functionality, but an operation
    /// requiring such functionality is attempted.
    /// </summary>
    public class NotStatefulException : StatefulException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotStatefulException"/> class with the specified sender.
        /// </summary>
        /// <param name="sender">The object that triggered the exception.</param>
        public NotStatefulException(object sender) : base("NotStatefulUser", SKTEOriginType.Inexternal, sender)
        { }
    }
}