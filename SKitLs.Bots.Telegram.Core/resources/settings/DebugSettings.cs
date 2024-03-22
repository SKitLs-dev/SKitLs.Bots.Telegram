using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Utils.Localizations.Model;
using SKitLs.Utils.Localizations.Prototype;
using SKitLs.Utils.LocalLoggers.Model;
using SKitLs.Utils.LocalLoggers.Prototype;

namespace SKitLs.Bots.Telegram.Core.resources.Settings
{
    /// <summary>
    /// Represents a class that holds global debug assets and settings.
    /// </summary>
    public class DebugSettings
    {
        /// <summary>
        /// Gets or sets the language used in debug output.
        /// </summary>
        public LangKey DebugLanguage { get; set; } = LangKey.EN;

        /// <summary>
        /// Represents the localization service used for retrieving localized debugging strings.
        /// <para/>
        /// The default localization service is <see cref="DefaultLocalizator"/>.
        /// </summary>
        public ILocalizator Localizator { get; private set; }

        /// <summary>
        /// Represents the logger service used for logging system messages.
        /// <para/>
        /// The default logger service is <see cref="LocalizedConsoleLogger"/>.
        /// </summary>
        public ILocalizedLogger LocalLogger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugSettings"/> class with specific data.
        /// </summary>
        /// <param name="language">The language of the debugging logger.</param>
        /// <param name="path">The path to the folder with localizations.</param>
        public DebugSettings(LangKey language = LangKey.EN, string path = "resources/locals")
        {
            DebugLanguage = language;
            Localizator = new DefaultLocalizator(path);
            LocalLogger = new LocalizedConsoleLogger(Localizator)
            {
                LoggerLanguage = DebugLanguage
            };
        }

        #region Settings

        /// <summary>
        /// Determines whether information about incoming updates should be printed.
        /// </summary>
        public bool LogUpdates { get; set; } = true;

        /// <summary>
        /// Determines whether information about thrown exceptions should be printed.
        /// </summary>
        public bool LogExceptions { get; set; } = true;

        /// <summary>
        /// Determines whether information about exceptions' stack trace should be printed.
        /// </summary>
        public bool LogExceptionTrace { get; set; } = false;
        #endregion

        /// <summary>
        /// Sets a custom path for debug localization, updating the localization service and logger.
        /// Still uses <see cref="DefaultLocalizator"/> and <see cref="LocalizedConsoleLogger"/>.
        /// </summary>
        /// <param name="path">The path to the folder with localized content.</param>
        public void UpdateLocalsPath(string path)
        {
            Localizator = new DefaultLocalizator(path);
            LocalLogger = new LocalizedConsoleLogger(Localizator);
        }

        /// <summary>
        /// Sets a custom debug localization using the provided localizator.
        /// </summary>
        /// <param name="localizator">The custom localizator to be used.</param>
        /// <exception cref="SKTgException"></exception>
        public void UpdateLocalsSystem(ILocalizator localizator)
        {
            Localizator = localizator;
            LocalLogger = new LocalizedConsoleLogger(localizator);
        }

        /// <summary>
        /// Sets a custom debug logger.
        /// </summary>
        /// <param name="localLogger">The custom logger to be used.</param>
        /// <exception cref="SKTgException"></exception>
        public void UpdateLogger(ILocalizedLogger localLogger)
        {
            LocalLogger = localLogger;
            Localizator = localLogger.Localizator;
        }
    }
}