using SKitLs.Bots.Telegram.Core.DeliverySystem;
using SKitLs.Bots.Telegram.Core.Interceptors;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Services;
using SKitLs.Bots.Telegram.Core.Services.Defaults;
using SKitLs.Bots.Telegram.Core.Settings;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Building
{
    /// <summary>
    /// Represents the entry point for the bot creation process.
    /// This class serves as a wizard constructor for the <see cref="BotManager"/> class.
    /// </summary>
    public class BotBuilder
    {
        /// <summary>
        /// Gets or sets the debug settings for the bot.
        /// </summary>
        public static DebugSettings DebugSettings { get; private set; }

        static BotBuilder() => DebugSettings = new();

        /// <summary>
        /// The constructed instance of the wizard constructor.
        /// </summary>
        private readonly BotManager _botManager;

        /// <summary>
        /// Creates a new instance of the wizard constructor.
        /// </summary>
        /// <param name="token">The Telegram bot's token.</param>
        /// <param name="settings">The bot's settings to be applied.</param>
        private BotBuilder(string token, DebugSettings? settings = null)
        {
            if (settings is not null)
                DebugSettings = settings;
            _botManager = new(token);
        }

        /// <summary>
        /// Creates a new instance of the wizard constructor.
        /// </summary>
        /// <param name="token">The Telegram bot's token.</param>
        public static BotBuilder NewBuilder(string token) => new(token);

        /// <summary>
        /// Enables handling of a specific chat type. Uses the vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="type">The type of chat to enable handling for.</param>
        /// <param name="designer">The customized <see cref="ChatScanner"/> designer. Set to <see langword="null"/> to use the default.</param>
        /// <returns>The current <see cref="BotBuilder"/> instance.</returns>
        public BotBuilder EnableChatsType(ChatType type, ChatDesigner? designer = null)
        {
            var scanner = designer?.Build() ?? new();
            scanner.ChatType = type;
            _botManager.ChatHandlers.Add(type, scanner);
            return this;
        }

        /// <summary>
        /// Enables handling of private chats. Uses the vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="designer">The customized <see cref="ChatScanner"/> designer. Set to <see langword="null"/> to use the default.</param>
        /// <returns>The current <see cref="BotBuilder"/> instance.</returns>
        public BotBuilder EnablePrivates(ChatDesigner? designer = null) => EnableChatsType(ChatType.Private, designer);

        /// <summary>
        /// Enables handling of groups. Uses the vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="designer">The customized <see cref="ChatScanner"/> designer. Can be <see langword="null"/> to use the default.</param>
        /// <returns>The current <see cref="BotBuilder"/> instance.</returns>
        public BotBuilder EnableGroups(ChatDesigner? designer = null) => EnableChatsType(ChatType.Group, designer);

        /// <summary>
        /// Enables handling of supergroups. Uses the vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="designer">The customized <see cref="ChatScanner"/> designer. Can be <see langword="null"/> to use the default.</param>
        /// <returns>The current <see cref="BotBuilder"/> instance.</returns>
        public BotBuilder EnableSupergroups(ChatDesigner? designer = null) => EnableChatsType(ChatType.Supergroup, designer);

        /// <summary>
        /// Enables handling of channels. Uses the vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="designer">The customized <see cref="ChatScanner"/> designer. Can be <see langword="null"/> to use the default.</param>
        /// <returns>The current <see cref="BotBuilder"/> instance.</returns>
        public BotBuilder EnableChannels(ChatDesigner? designer = null) => EnableChatsType(ChatType.Channel, designer);

        /// <summary>
        /// Sets a custom <see cref="IDeliveryService"/> for message delivery.
        /// </summary>
        /// <param name="delivery">The custom service to be implemented.</param>
        public BotBuilder CustomDelivery(IDeliveryService delivery)
        {
            _botManager.DeliveryService = delivery;
            return this;
        }

        /// <summary>
        /// Adds a new service of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The interface type of the service.</typeparam>
        /// <param name="service">The service to be stored.</param>
        public BotBuilder AddService<T>(T service) where T : notnull, IBotService
        {
            _botManager.AddService(service);
            return this;
        }

        /// <summary>
        /// Adds an update interceptor to the bot builder.
        /// </summary>
        /// <param name="updateInterceptor">The update interceptor to add.</param>
        public BotBuilder AddInterceptor(IUpdateInterceptor updateInterceptor)
        {
            _botManager.AddInterceptor(updateInterceptor);
            return this;
        }

        /// <summary>
        /// Compiles the created instance and returns the built one.
        /// </summary>
        /// <param name="debugName">The custom debug name (<see cref="BotManager.DebugName"/>).</param>
        public BotManager Build(string? debugName = null)
        {
            if (debugName is not null)
                _botManager.DebugName = debugName;
            _botManager.AddService<ILocalizatorService>(new LocalizatorService(DebugSettings.Localizator));
            _botManager.AddService<ILocalLoggerService>(new LocalLoggerService(DebugSettings.LocalLogger));
            _botManager.ReflectiveCompile();
            _botManager.CollectActionsBasket();
            return _botManager;
        }
    }
}