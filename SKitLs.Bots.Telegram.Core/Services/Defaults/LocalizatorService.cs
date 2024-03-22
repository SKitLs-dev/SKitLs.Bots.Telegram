using SKitLs.Bots.Telegram.Core.Model.Services;
using SKitLs.Utils.Localizations.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.Services.Defaults
{
    /// <summary>
    /// Represents a service that implements <see cref="IBotService"/> for handling localization through <see cref="ILocalizator"/> interface.
    /// It provides a default implementation for resolving localized strings.
    /// </summary>
    public interface ILocalizatorService : IBotService, ILocalizator
    {

    }

    /// <summary>
    /// Represents a service that implements <see cref="IBotService"/> for handling localization through <see cref="ILocalizator"/> interface.
    /// It provides a default implementation for resolving localized strings.
    /// </summary>
    public class LocalizatorService : BotServiceBase, ILocalizatorService
    {
        /// <summary>
        /// The localizator instance used for resolving localized strings.
        /// </summary>
        public ILocalizator Localizator { get; set; }

        /// <inheritdoc/>
        public string NotDefinedKey => Localizator.NotDefinedKey;

        /// <inheritdoc/>
        public string LocalsPath => Localizator.LocalsPath;

        /// <inheritdoc/>
        public string ResolveString(LangKey lang, string key, params string?[] format) => Localizator.ResolveString(lang, key, format);

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizatorService"/> class with the specified localizator.
        /// </summary>
        /// <param name="localizator">The localizator instance to use.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="localizator"/> is <see langword="null"/>.</exception>
        public LocalizatorService(ILocalizator localizator)
        {
            Localizator = localizator ?? throw new ArgumentNullException(nameof(localizator));
        }
    }
}
