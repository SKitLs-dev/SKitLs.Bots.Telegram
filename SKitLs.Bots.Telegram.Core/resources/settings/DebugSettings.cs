using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Utils.Localizations.Model;
using SKitLs.Utils.Localizations.Prototype;
using SKitLs.Utils.LocalLoggers.Model;
using SKitLs.Utils.LocalLoggers.Prototype;

namespace SKitLs.Bots.Telegram.Core.resources.Settings
{
    /// <summary>
    /// Represents a class with global debug assets.
    /// </summary>
    public class DebugSettings
    {
        /// <summary>
        /// Language that is used in debug output.
        /// </summary>
        public LangKey DebugLanguage { get; set; } = LangKey.EN;
        /// <summary>
        /// Localization service used for getting localized debugging strings.
        /// <para>
        /// <see cref="DefaultLocalizator"/> by default.
        /// </para>
        /// </summary>
        public ILocalizator Localizator { get; private set; }
        /// <summary>
        /// Logger service used for logging system messages.
        /// <para>
        /// <see cref="LocalizedConsoleLogger"/> by default.
        /// </para>
        /// </summary>
        public ILocalizedLogger LocalLogger { get; private set; }
        /// <summary>
        /// Creates a new instance of <see cref="DebugSettings"/> with specific data.
        /// </summary>
        /// <param name="language">Language of debugging logger.</param>
        /// <param name="path">Path to folder with localizations.</param>
        public DebugSettings(LangKey language = LangKey.EN, string path = "resources/locals")
        {
            DebugLanguage = language;
            Localizator = new DefaultLocalizator(path);
            LocalLogger = new LocalizedConsoleLogger(Localizator)
            {
                LoggerLanguage = DebugLanguage
            };
        }

        #region Bot Manager
        /// <summary>
        /// Determines whether information about incoming updates should be printed.
        /// </summary>
        public bool ShouldPrintUpdates { get; set; } = true;
        /// <summary>
        /// Determines whether information about thrown exceptions should be printed.
        /// </summary>
        public bool ShouldPrintExceptions { get; set; } = true;
        /// <summary>
        /// Determines whether information about exceptions' stack trace should be printed.
        /// </summary>
        public bool ShouldPrintExceptionTrace { get; set; } = false;
        #endregion

        /// <summary>
        /// Sets custom path for debug localization.
        /// Still uses <see cref="DefaultLocalizator"/> and <see cref="LocalizedConsoleLogger"/>.
        /// </summary>
        /// <param name="path">Path to folder with localized content.</param>
        public void UpdateLocalsPath(string path)
        {
            Localizator = new DefaultLocalizator(path);
            LocalLogger = new LocalizedConsoleLogger(Localizator);
        }
        /// <summary>
        /// Sets custom debug localization.
        /// </summary>
        /// <param name="localLogger">Custom logger.</param>
        /// <exception cref="SKTgException"></exception>
        public void UpdateLocalsSystem(ILocalizedLogger localLogger)
        {
            LocalLogger = localLogger;
            Localizator = localLogger.Localizator;
        }
    }
}