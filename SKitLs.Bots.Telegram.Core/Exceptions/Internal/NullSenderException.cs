namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    /// <summary>
    /// An exception that occurs when an attempt to retrieve a certain user has failed.
    /// </summary>
    public class NullSenderException : SKTgSignedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullSenderException"/> class with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown the exception.</param>
        public NullSenderException(object sender) : base("NullSender", SKTEOriginType.Internal, sender) { }
    }
}