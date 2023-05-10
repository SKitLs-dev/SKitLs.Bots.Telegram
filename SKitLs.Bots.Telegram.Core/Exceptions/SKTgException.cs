namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public class SKTgException : Exception
    {
        public bool ShouldBeNotified { get; private set; }

        private static string LocalKeyPrefix => "exception";
        public string KeyBase { get; private set; }
        public string LocalKey => $"{LocalKeyPrefix}.{KeyBase}";

        public string?[] Format { get; set; }

        public SKTgException(bool notify, string localKey, params string?[] format)
        {
            ShouldBeNotified = notify;
            KeyBase = localKey;
            Format = format;
        }
    }
}