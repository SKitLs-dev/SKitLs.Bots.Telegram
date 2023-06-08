namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public enum SKTEOriginType
    {
        Internal = -10,
        Inexternal = 0,
        External = 10,
    }

    public class SKTgException : Exception
    {
        public SKTEOriginType OriginType { get; private set; }

        private static string LocalKeyPrefix => "exception";
        public string KeyBase { get; private set; }
        public string CaptionLocalKey => $"{LocalKeyPrefix}Cap.{KeyBase}";
        public string MessgeLocalKey => $"{LocalKeyPrefix}Mes.{KeyBase}";

        public string?[] Format { get; protected set; }

        public SKTgException(string localKey, SKTEOriginType originType, params string?[] format)
        {
            OriginType = originType;
            KeyBase = localKey;
            Format = format;
        }
    }
}