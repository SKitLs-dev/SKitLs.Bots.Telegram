using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Exceptions
{
    public class ConvertingException : SKTgException
    {
        public ConvertingException() : base(true, "exceptions")
        {
        }
    }
}