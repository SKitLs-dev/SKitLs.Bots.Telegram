namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public class SKTgException : Exception
    {
        private static string LocalKeyPrefix => "exception";
        public bool ShouldBeNotified { get; private set; }
        public string LocalKey { get; private set; }

        public SKTgException(bool notify, string localKey)
        {
            ShouldBeNotified = notify;
            LocalKey = localKey;
        }
    }
}
