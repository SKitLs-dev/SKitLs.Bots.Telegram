using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.Inexternal
{
    public class ConvertNullInputException : SKTgException
    {
        public ConvertNullInputException(params string?[] format) : base("ConvertNullInput", SKTEOriginType.Inexternal, format)
        { }
    }
}