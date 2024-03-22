using SKitLs.Bots.Telegram.Core.Model.Services;
using SKitLs.Utils.Localizations.Prototype;
using SKitLs.Utils.LocalLoggers.Prototype;
using SKitLs.Utils.Loggers.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.Services.Defaults
{
    /// <summary>
    /// Represents a service that implements <see cref="IBotService"/> for handling localization-aware logging through <see cref="ILocalizedLogger"/> interface.
    /// </summary>
    public interface ILocalLoggerService : IBotService, ILocalizedLogger
    {

    }

    /// <summary>
    /// Represents a service that implements <see cref="IBotService"/> for handling localization-aware logging through <see cref="ILocalizedLogger"/> interface.
    /// </summary>
    public class LocalLoggerService : BotServiceBase, ILocalLoggerService
    {
        /// <summary>
        /// The localized logger instance associated with this service.
        /// </summary>
        public ILocalizedLogger LocalizedLogger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalLoggerService"/> class with the specified <paramref name="localizedLogger"/>.
        /// </summary>
        /// <param name="localizedLogger">The localized logger instance to be associated with this service.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="localizedLogger"/> is <see langword="null"/>.</exception>
        public LocalLoggerService(ILocalizedLogger localizedLogger)
        {
            LocalizedLogger = localizedLogger ?? throw new ArgumentNullException(nameof(localizedLogger));
        }

        /// <inheritdoc/>
        public LangKey LoggerLanguage => LocalizedLogger.LoggerLanguage;

        /// <inheritdoc/>
        public ILocalizator Localizator => LocalizedLogger.Localizator;

        /// <inheritdoc/>
        public void DropEmpty() => LocalizedLogger.DropEmpty();

        /// <inheritdoc/>
        public void Log(string message, LogType logType = LogType.Message, bool standsAlone = true) => LocalizedLogger.Log(message, logType, standsAlone);

        /// <inheritdoc/>
        public void Inform(string message, bool standsAlone = true) => LocalizedLogger.Inform(message, standsAlone);

        /// <inheritdoc/>
        public void Success(string message, bool standsAlone = true) => LocalizedLogger.Success(message, standsAlone);

        /// <inheritdoc/>
        public void Warn(string message, bool standsAlone = true) => LocalizedLogger.Warn(message, standsAlone);

        /// <inheritdoc/>
        public void Error(string message, bool standsAlone = true) => LocalizedLogger.Error(message, standsAlone);

        /// <inheritdoc/>
        public void System(string message, bool standsAlone = true) => LocalizedLogger.System(message, standsAlone);

        /// <inheritdoc/>
        public void LLog(string mesKey, LogType logType = LogType.Message, bool standsAlone = true, params string?[] format) => LocalizedLogger.LLog(mesKey, logType, standsAlone, format);

        /// <inheritdoc/>
        public void LInform(string mesKey, bool standsAlone = true, params string?[] format) => LocalizedLogger.LInform(mesKey, standsAlone, format);

        /// <inheritdoc/>
        public void LSuccess(string mesKey, bool standsAlone = true, params string?[] format) => LocalizedLogger.LSuccess(mesKey, standsAlone, format);

        /// <inheritdoc/>
        public void LWarn(string mesKey, bool standsAlone = true, params string?[] format) => LocalizedLogger.LWarn(mesKey, standsAlone, format);

        /// <inheritdoc/>
        public void LError(string mesKey, bool standsAlone = true, params string?[] format) => LocalizedLogger.LError(mesKey, standsAlone, format);

        /// <inheritdoc/>
        public void LSystem(string mesKey, bool standsAlone = true, params string?[] format) => LSystem(mesKey, standsAlone, format);
    }
}
