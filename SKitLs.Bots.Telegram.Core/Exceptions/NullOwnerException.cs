namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public class NullOwnerException : SKTgException
    {
        public NullOwnerException() : base(true, "exception_NullOwner") { }
    }
}