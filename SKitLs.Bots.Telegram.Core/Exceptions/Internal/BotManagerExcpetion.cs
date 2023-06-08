namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    public class BotManagerExcpetion : SKTgException
    {
        public BotManagerExcpetion(string localKey, params string?[] format)
            : base(localKey, SKTEOriginType.Internal, format) { }
    }
}