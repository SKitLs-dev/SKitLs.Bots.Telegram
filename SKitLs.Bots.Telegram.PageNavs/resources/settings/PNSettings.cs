namespace SKitLs.Bots.Telegram.PageNavs.resources.settings
{
    public static class PNSettings
    {
        public static string LibraryKeyPrefix { get; set; } = "pn.";

        public static string _backButtonLocalKey = "display.BackButton";
        /// <summary>
        /// A system callback label key, used for <see cref="BackCallabck"/>.
        /// </summary>
        public static string BackButtonLocalKey
        {
            get => LibraryKeyPrefix + _backButtonLocalKey;
            set => _backButtonLocalKey = value;
        }

        public static string _exitButtonLocalKey = "display.ExitButton";
        /// <summary>
        /// A system callback label key, used for [ <see cref="OpenPageCallabck"/> (X) Exit Funcs ].
        /// </summary>
        public static string ExitButtonLocalKey
        {
            get => LibraryKeyPrefix + _exitButtonLocalKey;
            set => _exitButtonLocalKey = value;
        }

        public static string _sessionExpiredLocalKey = "display.SessionExpired";
        public static string SessionExpiredLocalKey
        {
            get => LibraryKeyPrefix + _sessionExpiredLocalKey;
            set => _sessionExpiredLocalKey = value;
        }
    }
}