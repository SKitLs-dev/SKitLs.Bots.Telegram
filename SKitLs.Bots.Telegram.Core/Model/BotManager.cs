using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.external.Localizations;
using SKitLs.Bots.Telegram.Core.external.LocalizedLoggers;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Core.resources.Settings;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// Main bot's manager. Recieves updates, handles and delegates them to sub-managers.
    /// <para>
    /// First architecture level.
    /// Lower: <see cref="ChatScanner"/>.
    /// </para>
    /// <para>Access this class by Wizard Builder <see cref="BotBuilder"/>.</para>
    /// </summary>
    public class BotManager : IDebugNamed
    {
        #region Properties
        public string? DebugName { get; set; }

        // TODO: use ChatType.GetValues() enum selector to make chats generic
        /// <summary>
        /// Bot's reactions in Private Chats.
        /// </summary>
        public ChatScanner? PrivateChatUpdateHandler { get; internal set; }
        /// <summary>
        /// Bot's reactions in Group Chats.
        /// </summary>
        public ChatScanner? GroupChatUpdateHandler { get; internal set; }
        /// <summary>
        /// Bot's reactions in Supergroup Chats.
        /// </summary>
        public ChatScanner? SupergroupChatUpdateHandler { get; internal set; }
        /// <summary>
        /// Bot's reactions in Channel Chats.
        /// </summary>
        public ChatScanner? ChannelChatUpdateHandler { get; internal set; }

        /// <summary>
        /// Telegram's bot token.
        /// </summary>
        internal string? Token { private get; set; }
        /// <summary>
        /// Shows either bot's token is declared.
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
        /// Language that is used by <see cref="IDelieveryService"/> by default to send custom localized preset system messages to user.
        /// Set up via <see cref="Settings"/>.
        /// </summary>
        public LangKey BotLanguage => Settings.BotLanguage;
        /// <summary>
        /// Language that is used by <see cref="LocalLogger"/> to print debug output.
        /// Set up via <see cref="BotBuilder.DebugSettings"/>
        /// </summary>
        public LangKey DebugLanguage => BotBuilder.DebugSettings.DebugLanguage;
        #endregion
        
        #region Services
        /// <summary>
        /// Simple IoC-Container for storing Singleton Services.
        /// </summary>
        private Dictionary<Type, object> Services { get; set; } = new();
        /// <summary>
        /// Adds a new service of a type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Interface type of a service</typeparam>
        /// <param name="service">Service to be stored</param>
        public void AddService<T>(T service) where T : notnull
        {
            if (Services.ContainsKey(typeof(T)))
                throw new DuplicationException(GetType(), typeof(T), $"{GetType().Name}.{nameof(AddService)}<T>()");
            Services.Add(typeof(T), service);
        }
        /// <summary>
        /// Gets stored service of a type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Interface type of a service</typeparam>
        /// <returns>Service of the requested type.</returns>
        /// <exception cref="ServiceNotDefinedException">Thrown when <typeparamref name="T"/> is not defined.</exception>
        public T ResolveService<T>() where T : notnull
        {
            if (!Services.ContainsKey(typeof(T))) throw new ServiceNotDefinedException(typeof(T));
            return (T)Services[typeof(T)];
        }

        /// <summary>
        /// Delievery Service used for sending messages <see cref="IBuildableMessage"/> to server.
        /// <para>
        /// <see cref="DefaultDelieveryService"/> by default.
        /// </para>
        /// </summary>
        public IDelieveryService DeliveryService { get; internal set; }
        /// <summary>
        /// Localization service used for getting localized debugging strings.
        /// <para>
        /// <see cref="DefaultLocalizator"/> by default.
        /// </para>
        /// </summary>
        [OwnerCompileIgnore]
        public ILocalizator Localizator => ResolveService<ILocalizator>();
        /// <summary>
        /// Gets localized text from <see cref="Localizator"/> using its unique key and <see cref="BotLanguage"/>.
        /// </summary>
        /// <param name="key">String's unique key.</param>
        /// <param name="format">The array of strings to format gotten one.</param>
        /// <returns>Formatted localized text.</returns>
        public string ResolveBotString(string key, params string?[] format) => Localizator.ResolveString(BotLanguage, key, format);
        /// <summary>
        /// Gets localized text from <see cref="Localizator"/> using its unique key and <see cref="DebugLanguage"/>.
        /// </summary>
        /// <param name="key">String's unique key.</param>
        /// <param name="format">The array of strings to format gotten one.</param>
        /// <returns>Formatted localized text.</returns>
        public string ResolveDebugString(string key, params string?[] format) => Localizator.ResolveString(DebugLanguage, key, format);
        /// <summary>
        /// Logger service used for logging system messages.
        /// <para>
        /// <see cref="DefaultLocalizedLogger"/> by default.
        /// </para>
        /// </summary>
        [OwnerCompileIgnore]
        public ILocalizedLogger LocalLogger => ResolveService<ILocalizedLogger>();
        #endregion

        /// <summary>
        /// Provides access to all declared <see cref="IBotAction"/>,
        /// collected via <see cref="IActionsHolder"/> inteface.
        /// </summary>
        public List<IBotAction> ActionsBusket { get; internal set; }
        /// <summary>
        /// Tries to find certain <see cref="IBotAction"/> by its id. Otherwise throws an Exception.
        /// </summary>
        /// <param name="actionId"><see cref="IBotAction"/>'s id</param>
        /// <returns>Declared <see cref="IBotAction"/> or <see cref="NotDefinedException"/> if doesn't exist.</returns>
        /// <exception cref="NotDefinedException"></exception>
        public IBotAction GetDeclaredAction(string actionId) => ActionsBusket
            .Find(x => x.ActionId == actionId)
            ?? throw new NotDefinedException(GetType(), typeof(IBotAction), actionId);

        internal BotManager(string token)
        {
            ActionsBusket = new();
            Settings = new();

            Token = token;
            Bot = new TelegramBotClient(token);
            DeliveryService = new DefaultDelieveryService();
        }

        /// <summary>
        /// Recursively and reflectively compiles all <see cref="IOwnerCompilable"/>, determined in the
        /// <see cref="BotManager"/> interior, inc. <see cref="Services"/>.
        /// <para>Summoned during <see cref="BotManager"/> building.
        /// See: <seealso cref="BotBuilder.Build(string?)"/></para>
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
        }

        /// <summary>
        /// Recursively and reflectively collects all declared <see cref="IBotAction"/>
        /// via <see cref="IActionsHolder"/> inteface.
        /// </summary>
        internal void CollectActionsBasket()
        {
            var holders = new List<IActionsHolder>();
            GetType().GetProperties()
                .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IActionsHolder)))
                .ToList()
                .ForEach(holder => holders.Add((holder.GetValue(this) as IActionsHolder)!));
            Services.Values.Where(x => x is IActionsHolder)
                .ToList()
                .ForEach(service => holders.Add((service as IActionsHolder)!));
            holders.ForEach(x =>
            {
                if (x is not null)
                    ActionsBusket.AddRange(x.GetActionsContent());
            });
        }

        /// <summary>
        /// Launches <see cref="ITelegramBotClient"/> <see cref="Bot"/> by starting server polling.
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
        /// Обрабатывает обновления, полученные от бота
        /// </summary>
        /// <param name="client"></param>
        /// <param name="update">Обновление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
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
        /// Handles bot exception.
        /// </summary>
        /// <param name="client">Client that raised an exception</param>
        /// <param name="exception">Exception</param>
        /// <param name="cancellationToken">Cancellation token</param>
        private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            if (BotBuilder.DebugSettings.ShouldPrintExceptions)
                BotBuilder.DebugSettings.LocalLogger.Log(exception);
            else BotBuilder.DebugSettings.LocalLogger.Error("Exception was handled.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Unpacks and casts raw data, delegating raw server's <see cref="Update"/> to
        /// sub-handlers in <see cref="ICastedUpdate"/> representation.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <exception cref="BotManagerExcpetion"></exception>
        private async Task SubDelegateUpdate(Update update)
        {
            if (BotBuilder.DebugSettings.ShouldPrintUpdates) BotBuilder.DebugSettings.LocalLogger.Log(update);

            long chatId = GetChatId(update);
            ChatType senderChatType = GetChatType(update);
            ChatScanner? _handler = senderChatType switch
            {
                ChatType.Private => PrivateChatUpdateHandler,
                ChatType.Group => GroupChatUpdateHandler,
                ChatType.Supergroup => SupergroupChatUpdateHandler,
                ChatType.Channel => ChannelChatUpdateHandler,
                _ => null,
            };

            if (_handler is null)
                throw new BotManagerExcpetion("bm.ChatTypeNotSupported", Enum.GetName(typeof(ChatType), senderChatType));

            await _handler.HandleUpdateAsync(new CastedUpdate(_handler, update, chatId));
        }

        /// <summary>
        /// Extracts <see cref="ChatType"/> from a raw server update or
        /// throws <see cref="BotManagerExcpetion"/> otherwise.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Update's chat type.</returns>
        public static ChatType GetChatType(Update update)
        {
            ChatType? senderChatType = TryGetChatType(update);
            if (senderChatType is null)
                throw new BotManagerExcpetion("bm.ChatNotHandled");
            return senderChatType.Value;
        }
        /// <summary>
        /// Tries to extract <see cref="ChatType"/> from a raw server update.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Update's chat type.</returns>
        public static ChatType? TryGetChatType(Update update)
        {
            // Polls ??
            // Shipping query ??
            if (update.CallbackQuery != null && update.CallbackQuery.Message != null)
                return update.CallbackQuery.Message.Chat?.Type;
            else if (update.ChannelPost != null)
                return update.ChannelPost.Chat?.Type;
            else if (update.ChatJoinRequest != null)
                return update.ChatJoinRequest.Chat?.Type;
            else if (update.ChatMember != null)
                return update.ChatMember.Chat?.Type;
            else if (update.EditedChannelPost != null)
                return update.EditedChannelPost.Chat?.Type;
            else if (update.EditedMessage != null)
                return update.EditedMessage.Chat?.Type;
            else if (update.InlineQuery != null)
                return update.InlineQuery.ChatType;
            else if (update.Message != null)
                return update.Message.Chat?.Type;
            else if (update.MyChatMember != null)
                return update.MyChatMember.Chat?.Type;

            else if (update.ChosenInlineResult != null)
                return ChatType.Sender;
            else return null;
        }

        /// <summary>
        /// Extracts chat's ID from a raw server update or
        /// throws <see cref="BotManagerExcpetion"/> otherwise.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Update's chat ID.</returns>
        public static long GetChatId(Update update)
        {
            long? chatId = TryGetChatId(update);
            if (chatId is null)
                throw new BotManagerExcpetion("bm.ChatIdNotHandled");
            return chatId.Value;
        }
        /// <summary>
        /// Tries to extract chat's ID from a raw server update.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Update's chat ID.</returns>
        public static long? TryGetChatId(Update update)
        {
            // Polls ??
            // Shipping query ??
            // Inline query ??
            // ChosenInlineResult ?? 
            if (update.CallbackQuery != null && update.CallbackQuery.Message != null)
                return update.CallbackQuery.Message.Chat?.Id;
            else if (update.ChannelPost != null)
                return update.ChannelPost.Chat?.Id;
            else if (update.ChatJoinRequest != null)
                return update.ChatJoinRequest.Chat?.Id;
            else if (update.ChatMember != null)
                return update.ChatMember.Chat?.Id;
            else if (update.EditedChannelPost != null)
                return update.EditedChannelPost.Chat?.Id;
            else if (update.EditedMessage != null)
                return update.EditedMessage.Chat?.Id;
            else if (update.Message != null)
                return update.Message.Chat?.Id;
            else if (update.MyChatMember != null)
                return update.MyChatMember.Chat?.Id;
            else return null;
        }

        public static List<UpdateType> PrivateUpdates { get; set; } = new()
        {
            UpdateType.Message,
            UpdateType.EditedMessage,
            UpdateType.CallbackQuery,
            UpdateType.InlineQuery,
            UpdateType.PreCheckoutQuery,
            UpdateType.ShippingQuery,
        };
        public static List<UpdateType> GroupUpdates { get; set; } = new()
        {
            UpdateType.Message,
            UpdateType.EditedMessage,
            UpdateType.CallbackQuery,
            UpdateType.InlineQuery,
            UpdateType.PreCheckoutQuery,
            UpdateType.ShippingQuery,

            UpdateType.ChatJoinRequest,
            UpdateType.ChatMember,
            UpdateType.MyChatMember,

            UpdateType.Poll,
            UpdateType.PollAnswer,
        };
        public static List<UpdateType> SupergroupUpdates { get; set; } = new()
        {
            UpdateType.Message,
            UpdateType.EditedMessage,
            UpdateType.CallbackQuery,
            UpdateType.InlineQuery,
            UpdateType.PreCheckoutQuery,
            UpdateType.ShippingQuery,

            UpdateType.ChatJoinRequest,
            UpdateType.ChatMember,
            UpdateType.MyChatMember,

            UpdateType.Poll,
            UpdateType.PollAnswer,
        };
        public static List<UpdateType> ChannelUpdates { get; set; } = new()
        {
            UpdateType.ChannelPost,
            UpdateType.EditedChannelPost,
            UpdateType.CallbackQuery,
            UpdateType.InlineQuery,

            UpdateType.ChatJoinRequest,
            UpdateType.ChatMember,
            UpdateType.MyChatMember,

            UpdateType.Poll,
            UpdateType.PollAnswer,
        };

        public override string? ToString() => DebugName ?? base.ToString();
    }
}