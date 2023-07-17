using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.external.Localizations;
using SKitLs.Bots.Telegram.Core.external.LocalizedLoggers;

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
        /// <see cref="DefaultLocalizedLogger"/> by default.
        /// </para>
        /// </summary>
        public ILocalizedLogger LocalLogger { get; private set; }
        /// <summary>
        /// Creates a new instance of <see cref="DebugSettings"/> with specific data.
        /// </summary>
        /// <param name="language">Language of debugging logger</param>
        /// <param name="path">Path to foler with localizations</param>
        public DebugSettings(LangKey language = LangKey.EN, string path = "resources/locals")
        {
            DebugLanguage = language;
            Localizator = new DefaultLocalizator(path);
            LocalLogger = new DefaultLocalizedLogger(Localizator)
            {
                DefaultLanguage = DebugLanguage
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
        /// Still uses <see cref="DefaultLocalizator"/> and <see cref="DefaultLocalizedLogger"/>.
        /// </summary>
        /// <param name="path">Path to folder with localized content</param>
        public void UpdateLocalsPath(string path)
        {
            Localizator = new DefaultLocalizator(path);
            LocalLogger = new DefaultLocalizedLogger(Localizator);
        }
        /// <summary>
        /// Sets custom debug localization. Note that <see cref="ILocalizedLogger.Localizator"/> should
        /// be similar with <paramref name="localizator"/>. Otherwise exception is thrown.
        /// </summary>
        /// <param name="localizator">Custom localizator</param>
        /// <param name="localLogger">Custom logger</param>
        /// <exception cref="SKTgException"></exception>
        public void UpdateLocalsSystem(ILocalizator localizator, ILocalizedLogger localLogger)
        {
            Localizator = localizator;
            if (localLogger.Localizator != localizator)
                throw new SKTgException("LocalSystemMissmatch", SKTEOriginType.External);
            LocalLogger = localLogger;
        }
    }
}