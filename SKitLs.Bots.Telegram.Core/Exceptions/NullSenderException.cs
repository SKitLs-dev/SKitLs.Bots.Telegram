namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public class NullSenderException : SKTgException
    {
        public NullSenderException() : base(true, "exception_NullSender")
        { }
    }
}