namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    /// <summary>
    /// An exception which occurs when attempt to get certain user was failed.
    /// </summary>
    public class NullSenderException : SKTgSignedException
    {
        /// <summary>
        /// Creates a new instance of <see cref="NullSenderException"/>.
        /// </summary>
        public NullSenderException(object sender) : base("NullSender", SKTEOriginType.Internal, sender) { }
    }
}