using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.DeliverySystem;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Interceptors;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.Services;
using SKitLs.Bots.Telegram.Core.Services.Defaults;
using SKitLs.Bots.Telegram.Core.Settings;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Utils.Localizations.Model;
using SKitLs.Utils.Localizations.Prototype;
using SKitLs.Utils.LocalLoggers.Model;
using SKitLs.Utils.LocalLoggers.Prototype;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// Main bot's manager. Receives updates, handles, and delegates them to sub-managers.
    /// <para/>
    /// This class represents the <b>first level</b> of the bot's architecture.
    /// <list type="number">
    ///     <item>
    ///         <b><see cref="BotManager"/></b>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ChatScanner"/></term>
    ///         <description>Used for handling updates in different chat types.</description>
    ///     </item>
    /// </list>
    /// Access this class by Wizard Builder <see cref="BotBuilder"/>.
    /// </summary>
    public sealed partial class BotManager : IDebugNamed
    {
        #region Properties

        private string? _debugName;
        /// <inheritdoc/>
        public string DebugName
        {
            get => _debugName ?? $"{nameof(BotManager)}";
            set => _debugName = value;
        }
        
        /// <summary>
        /// Represents a dictionary that maps each <see cref="ChatType"/> to its corresponding <see cref="ChatScanner"/>.
        /// </summary>
        public Dictionary<ChatType, ChatScanner> ChatHandlers { get; private init; } = new();

        /// <summary>
        /// Telegram's bot token.
        /// </summary>
        internal string? Token { private get; set; }

        /// <summary>
        /// Shows whether the bot's token is declared.
        /// </summary>
        public bool IsTokenDefined => Token is not null;

        /// <summary>
        /// External essential bot's instance.
        /// </summary>
        public ITelegramBotClient Bot { get; private set; }
        #endregion

        #region Settings
        
        /// <summary>
        /// Bot's common settings.
        /// </summary>
        public BotSettings Settings { get; private set; }

        /// <summary>
        /// Language that is used by <see cref="IDeliveryService"/> by default to send custom localized preset system messages to the user.
        /// Set up via <see cref="Settings"/>.
        /// </summary>
        public LangKey BotLanguage => Settings.BotLanguage;

        /// <summary>
        /// Language that is used by <see cref="LocalLogger"/> to print debug output.
        /// Set up via <see cref="BotBuilder.DebugSettings"/>.
        /// </summary>
        public static LangKey DebugLanguage => BotBuilder.DebugSettings.DebugLanguage;
        #endregion

        #region Services

        /// <summary>
        /// Gets or sets the list of update interceptors.
        /// </summary>
        private List<IUpdateInterceptor> Interceptors { get; set; } = [];

        /// <summary>
        /// Adds an update interceptor to the manager.
        /// </summary>
        /// <param name="interceptor">The update interceptor to add.</param>
        public void AddInterceptor(IUpdateInterceptor interceptor) => Interceptors.Add(interceptor);

        /// <summary>
        /// Simple IoC-Container for storing Singleton Services.
        /// </summary>
        private Dictionary<Type, object> Services { get; set; } = [];

        /// <summary>
        /// Adds a new service of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Interface type of the service.</typeparam>
        /// <param name="service">Service to be stored.</param>
        public void AddService<T>(T service) where T : notnull, IBotService
        {
            if (Services.ContainsKey(typeof(T)))
                throw new DuplicationException(GetType(), typeof(T), $"{GetType().Name}.{nameof(AddService)}<T>()");
            Services.Add(typeof(T), service);
        }

        /// <summary>
        /// Gets the stored service of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Interface type of the service.</typeparam>
        /// <returns>Service of the requested type.</returns>
        /// <exception cref="ServiceNotDefinedException">Thrown when <typeparamref name="T"/> is not defined.</exception>
        public T ResolveService<T>() where T : notnull, IBotService
        {
            if (!Services.ContainsKey(typeof(T))) throw new ServiceNotDefinedException(this, typeof(T));
            return (T)Services[typeof(T)];
        }

        /// <summary>
        /// Delivery Service used for sending messages (<see cref="ITelegramMessage"/>) to the server.
        /// <para/>
        /// Defaults to <see cref="DefaultDeliveryService"/>.
        /// </summary>
        public IDeliveryService DeliveryService { get; internal set; }

        /// <summary>
        /// Provides localization services for obtaining localized debugging strings.
        /// <para/>
        /// <b>Ignored on compilation.</b> Defaults to <see cref="DefaultLocalizator"/>.
        /// </summary>
        [OwnerCompileIgnore]
        public ILocalizator Localizator => ResolveService<ILocalizatorService>();

        /// <summary>
        /// Gets localized text from <see cref="Localizator"/> using its unique key and <see cref="BotLanguage"/> language.
        /// </summary>
        /// <param name="key">String's unique key.</param>
        /// <param name="format">The array of strings to format the gotten one.</param>
        /// <returns>Formatted localized text.</returns>
        public string ResolveBotString(string key, params string?[] format) => Localizator.ResolveString(BotLanguage, key, format);

        /// <summary>
        /// Gets localized text from <see cref="Localizator"/> using its unique key and <see cref="DebugLanguage"/>.
        /// </summary>
        /// <param name="key">String's unique key.</param>
        /// <param name="format">The array of strings to format the gotten one.</param>
        /// <returns>Formatted localized text.</returns>
        public string ResolveDebugString(string key, params string?[] format) => Localizator.ResolveString(DebugLanguage, key, format);

        /// <summary>
        /// Logger service used for logging system messages.
        /// <para>
        /// <b>Ignored on compilation.</b> Defaults to <see cref="LocalizedConsoleLogger"/>.
        /// </para>
        /// </summary>
        [OwnerCompileIgnore]
        public ILocalizedLogger LocalLogger => ResolveService<ILocalLoggerService>();
        #endregion
        
        /// <summary>
        /// Provides access to all declared <see cref="IBotAction"/>,
        /// collected via <see cref="IBotActionsHolder"/> interface.
        /// </summary>
        public List<IBotAction> ActionsBasket { get; internal set; }
        /// <summary>
        /// Tries to find certain <see cref="IBotAction"/> by its id. Otherwise throws an Exception.
        /// </summary>
        /// <param name="actionId"><see cref="IBotAction"/>'s id.</param>
        /// <returns>Declared <see cref="IBotAction"/> or <see cref="NotDefinedException"/> if doesn't exist.</returns>
        /// <exception cref="NotDefinedException"></exception>
        public IBotAction GetDeclaredAction(string actionId) => ActionsBasket
            .Find(x => x.ActionId == actionId)
            ?? throw new NotDefinedException(GetType(), typeof(IBotAction), actionId);

        /// <summary>
        /// Initializes a new instance of the <see cref="BotManager"/> class with the specified bot token.
        /// </summary>
        /// <param name="token">The Telegram bot token.</param>
        internal BotManager(string token)
        {
            ActionsBasket = new();
            Settings = new();

            Token = token;
            Bot = new TelegramBotClient(token);
            DeliveryService = new DefaultDeliveryService();
        }

        /// <summary>
        /// Recursively and reflectively compiles all <see cref="IOwnerCompilable"/> instances defined within the <see cref="BotManager"/>.
        /// <para/>
        /// Invoked during the building process of the <see cref="BotManager"/>. See <seealso cref="BotBuilder.Build(string?)"/>.
        /// </summary>
        internal void ReflectiveCompile()
        {
            GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttribute<OwnerCompileIgnoreAttribute>() is null)
                .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IOwnerCompilable)))
                .ToList()
                .ForEach(refCompile =>
                {
                    var cmpVal = refCompile.GetValue(this);
                    if (cmpVal is IOwnerCompilable oc)
                        oc.ReflectiveCompile(cmpVal, this);
                });

            Services.Values.Where(x => x is IOwnerCompilable)
                .ToList()
                .ForEach(service => (service as IOwnerCompilable)!.ReflectiveCompile(service, this));

            Interceptors.Where(x => x is IOwnerCompilable)
                .ToList()
                .ForEach(service => (service as IOwnerCompilable)!.ReflectiveCompile(service, this));
        }

        /// <summary>
        /// Recursively and reflectively collects all declared <see cref="IBotAction"/> instances via the <see cref="IBotActionsHolder"/> interface.
        /// </summary>
        internal void CollectActionsBasket()
        {
            var holders = new List<IBotActionsHolder>();
            GetType().GetProperties()
                .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IBotActionsHolder)))
                .ToList()
                .ForEach(holder => holders.Add((holder.GetValue(this) as IBotActionsHolder)!));
            Services.Values.Where(x => x is IBotActionsHolder)
                .ToList()
                .ForEach(service => holders.Add((service as IBotActionsHolder)!));
            holders.ForEach(x =>
            {
                if (x is not null)
                    ActionsBasket.AddRange(x.GetHeldActions());
            });
        }

        /// <summary>
        /// Launches the bot by starting the asynchronous server polling using <see cref="ITelegramBotClient"/>.
        /// </summary>
        public async Task Listen()
        {
            var cts = new CancellationTokenSource();
            try
            {
                var me = await Bot.GetMeAsync();
                BotBuilder.DebugSettings.LocalLogger.LSuccess("system.StartUpMessage", format: me.Username);

                await Bot.ReceiveAsync(
                    new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                    cancellationToken: cts.Token);
            }
            catch (Exception exception)
            {
                BotBuilder.DebugSettings.LocalLogger.Log(exception);
                cts.Cancel();
            }
        }

        /// <summary>
        /// Asynchronously handles the updates received from the bot.
        /// </summary>
        /// <param name="client">The Telegram bot client.</param>
        /// <param name="update">The received update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            try
            {
                await SubDelegateUpdate(update);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(client, exception, cancellationToken);
            }
        }

        /// <summary>
        /// Handles bot exceptions.
        /// </summary>
        /// <param name="client">The Telegram bot client.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            if (BotBuilder.DebugSettings.LogExceptions)
                BotBuilder.DebugSettings.LocalLogger.Log(exception);
            else BotBuilder.DebugSettings.LocalLogger.Error("Exception was handled.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Unpacks and casts raw data, delegating the raw server's <see cref="Update"/> to sub-handlers in the <see cref="ICastedUpdate"/> representation.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="BotManagerException">Thrown when the chat type is not supported.</exception>
        private async Task SubDelegateUpdate(Update update)
        {
            if (BotBuilder.DebugSettings.LogUpdates)
                BotBuilder.DebugSettings.LocalLogger.Log(update);

            foreach (var interceptor in Interceptors)
                if (interceptor.ShouldIntercept(update))
                    if (interceptor.HandleUpdate(update))
                        return;

            var chatId = TelegramHelper.GetChatId(update, this);
            var senderChatType = TelegramHelper.GetChatType(update, this);
            if (ChatHandlers.TryGetValue(senderChatType, out ChatScanner? _handler))
            {
                await _handler.HandleUpdateAsync(new CastedUpdate(_handler, update, chatId));
            }
            else
                throw new BotManagerException("bm.ChatTypeNotSupported", this, Enum.GetName(typeof(ChatType), senderChatType));
        }

        /// <inheritdoc/>
        public override string? ToString() => DebugName ?? base.ToString();
    }
}