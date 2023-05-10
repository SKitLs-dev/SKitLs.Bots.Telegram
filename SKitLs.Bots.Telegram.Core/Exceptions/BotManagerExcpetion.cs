namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public class BotManagerExcpetion : SKTgException
    {
        public BotManagerExcpetion(bool notify, string localKey, params string?[] format)
            : base(notify, localKey, format) { }
    }
}