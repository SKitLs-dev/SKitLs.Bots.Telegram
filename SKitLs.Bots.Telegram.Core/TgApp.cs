using SKitLs.Bots.Telegram.Core.external.Localizations;
using SKitLs.Bots.Telegram.Core.resources.Settings;

namespace SKitLs.Bots.Telegram.Core
{
    /// <summary>
    /// Application class. Hosts and realeses services. Use DI model.
    /// </summary>
    public static class TgApp
    {
        /// <summary>
        /// Bot's common settings
        /// </summary>
        public static BotSettings Settings { get; private set; }
        /// <summary>
        /// Bot's debugging settings
        /// </summary>
        public static DebugSettings DebugSettings { get; private set; }

        public static ILocalizator Localizator { get; set; }

        static TgApp()
        {
            Localizator = new Localizator();

            DebugSettings = DebugSettings.Default();
            Settings = new();
        }
    }
}