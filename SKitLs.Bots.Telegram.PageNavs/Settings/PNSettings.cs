using SKitLs.Bots.Telegram.PageNavs.Model;

namespace SKitLs.Bots.Telegram.PageNavs.Settings
{
    /// <summary>
    /// Represents special settings class for <c>*.PageNavs</c> project.
    /// </summary>
    public static class PNSettings
    {
        /// <summary>
        /// Represents key prefix for library's localizations.
        /// </summary>
        public static string LibraryKeyPrefix { get; set; } = "pn.";

        /// <summary>
        /// A system callback name, used for <see cref="IMenuService.OpenPageCallback"/>.
        /// </summary>
        public static string OpenCallBase => "OpenMenuPage";
        /// <summary>
        /// A system callback name, used for <see cref="IMenuService.BackCallback"/>.
        /// </summary>
        public static string BackCallBase => "BackMenuPage";

        private static string _backButtonLK = "display.BackButton";
        /// <summary>
        /// A system callback label key, used for <see cref="IMenuService.BackCallback"/>.
        /// </summary>
        public static string BackButtonLocalKey
        {
            get => LibraryKeyPrefix + _backButtonLK;
            set => _backButtonLK = value;
        }

        private static string _exitButtonLK = "display.ExitButton";
        /// <summary>
        /// A system callback label key, used for <see cref="IMenuService.OpenPageCallback"/>.
        /// </summary>
        public static string ExitButtonLocalKey
        {
            get => LibraryKeyPrefix + _exitButtonLK;
            set => _exitButtonLK = value;
        }

        private static string _sessionExpiredLK = "display.SessionExpired";
        /// <summary>
        /// Represents localization key for a system message, which occurs when session data is expired.
        /// </summary>
        public static string SessionExpiredLocalKey
        {
            get => LibraryKeyPrefix + _sessionExpiredLK;
            set => _sessionExpiredLK = value;
        }
    }
}