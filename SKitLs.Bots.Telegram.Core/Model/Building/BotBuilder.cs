using SKitLs.Bots.Telegram.Core.Model.DeliverySystem;
using SKitLs.Bots.Telegram.Core.resources.Settings;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.Building
{
    /// <summary>
    /// Bot creating process enter point. <see cref="BotManager"/> class wizard constructor.
    /// </summary>
    public class BotBuilder
    {
        /// <summary>
        /// Bot's debug settings.
        /// </summary>
        public static DebugSettings DebugSettings { get; private set; }

        static BotBuilder() => DebugSettings = new();

        /// <summary>
        /// Constructing instance.
        /// </summary>
        private readonly BotManager _botManager;
        /// <summary>
        /// Creates a new instance of the wizard constructor.
        /// </summary>
        /// <param name="token">Telegram bot's token.</param>
        /// <param name="settings">Bot's settings to be applied.</param>
        private BotBuilder(string token, DebugSettings? settings = null)
        {
            if (settings is not null)
                DebugSettings = settings;
            _botManager = new(token);
        }
        /// <summary>
        /// Creates a new instance of the wizard constructor.
        /// </summary>
        /// <param name="token">Telegram bot's token.</param>
        public static BotBuilder NewBuilder(string token) => new(token);

        /// <summary>
        /// Enables private chats' handling. Uses vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="builder">Customized <see cref="ChatScanner"/> designer.
        /// Set <see langword="null"/> to use default.</param>
        public BotBuilder EnablePrivates(ChatDesigner? builder = null)
        {
            _botManager.PrivateChatUpdateHandler = builder?.Build() ?? new();
            _botManager.PrivateChatUpdateHandler.ChatType = ChatType.Private;
            _botManager.PrivateChatUpdateHandler.DebugName = nameof(_botManager.PrivateChatUpdateHandler);
            return this;
        }
        /// <summary>
        /// Enables groups' handling. Uses vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="builder">Customized <see cref="ChatScanner"/> designer. Can be null to use default.</param>
        public BotBuilder EnableGroups(ChatDesigner? builder = null)
        {
            _botManager.GroupChatUpdateHandler = builder?.Build() ?? new();
            _botManager.GroupChatUpdateHandler.ChatType = ChatType.Group;
            _botManager.GroupChatUpdateHandler.DebugName = nameof(_botManager.GroupChatUpdateHandler);
            return this;
        }
        /// <summary>
        /// Enables supergroups' handling. Uses vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="builder">Customized <see cref="ChatScanner"/> designer. Can be null to use default.</param>
        public BotBuilder EnableSupergroups(ChatDesigner? builder = null)
        {
            _botManager.SupergroupChatUpdateHandler = builder?.Build() ?? new();
            _botManager.SupergroupChatUpdateHandler.ChatType = ChatType.Supergroup;
            _botManager.SupergroupChatUpdateHandler.DebugName = nameof(_botManager.SupergroupChatUpdateHandler);
            return this;
        }
        /// <summary>
        /// Enables channels' handling. Uses vanilla <see cref="ChatScanner"/> by default.
        /// </summary>
        /// <param name="builder">Customized <see cref="ChatScanner"/> designer. Can be null to use default.</param>
        public BotBuilder EnableChannels(ChatDesigner? builder = null)
        {
            _botManager.ChannelChatUpdateHandler = builder?.Build() ?? new();
            _botManager.ChannelChatUpdateHandler.ChatType = ChatType.Channel;
            _botManager.ChannelChatUpdateHandler.DebugName = nameof(_botManager.ChannelChatUpdateHandler);
            return this;
        }

        /// <summary>
        /// Sets custom <see cref="IDeliveryService"/> for messages sending.
        /// </summary>
        /// <param name="delivery">Custom service to be implemented.</param>
        public BotBuilder CustomDelivery(IDeliveryService delivery)
        {
            _botManager.DeliveryService = delivery;
            return this;
        }

        /// <summary>
        /// Adds a new service of a type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Interface type of a service</typeparam>
        /// <param name="service">Service to be stored.</param>
        public BotBuilder AddService<T>(T service) where T : notnull
        {
            _botManager.AddService(service);
            return this;
        }

        /// <summary>
        /// Compiles created instance and returns the built one.
        /// </summary>
        /// <param name="debugName">Custom debug name (<see cref="BotManager.DebugName"/>).</param>
        public BotManager Build(string? debugName = null)
        {
            _botManager.DebugName = debugName;
            _botManager.ReflectiveCompile();
            _botManager.CollectActionsBasket();
            _botManager.AddService(DebugSettings.Localizator);
            _botManager.AddService(DebugSettings.LocalLogger);
            return _botManager;
        }
    }
}