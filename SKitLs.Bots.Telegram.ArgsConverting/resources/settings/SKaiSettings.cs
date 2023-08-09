using System.Text;

namespace SKitLs.Bots.Telegram.ArgedInteractions.resources.settings
{
    /// <summary>
    /// The <see cref="SKaiSettings"/> class provides settings related to the SKitLs.Bots.Telegram.ArgedInteractions extension.
    /// </summary>
    public static class SKaiSettings
    {
        /// <summary>
        /// Gets or sets the prefix used for the extension's localizations.
        /// Default value is "ai".
        /// </summary>
        public static string ExtensionPrefix { get; set; } = "ai";
        
        // TODO: Extract .display
        private static string _argumentsCountMissMatchLK = "display.ArgumentsCountMissMatch";
        /// <summary>
        /// Gets or sets the localization key for the "Arguments Count Mismatch" error message.
        /// This key is used to retrieve the corresponding error message string for display.
        /// </summary>
        public static string ArgumentsCountMissMatchLK
        {
            get => new StringBuilder().AppendJoin('.', ExtensionPrefix, _argumentsCountMissMatchLK).ToString();
            set => _argumentsCountMissMatchLK = value;
        }
    }
}