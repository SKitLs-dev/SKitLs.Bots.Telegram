namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    public class NullSenderException : SKTgException
    {
        public NullSenderException() : base("NullSender", SKTEOriginType.Internal) { }
    }
}