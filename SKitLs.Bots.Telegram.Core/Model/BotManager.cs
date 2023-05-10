using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.external.Localizations;
using SKitLs.Bots.Telegram.Core.external.LocalizedLoggers;
using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.resources.Settings;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// Main bot's manager. Recieves updates, handles and delegates them to sub-managers.
    /// <para>Access this class by Wizard Builder <see cref="BotBuilder"/>.</para>
    /// </summary>
    public class BotManager
    {
        #region Properties
        /// <summary>
        /// Used for simplified dubugging proccess. Each BotManager can be named.
        /// </summary>
        public string? DebugName { get; set; }

        /// <summary>
        /// Bot's reactions in Private Chats.
        /// </summary>
        public ChatScanner? PrivateChatUpdateHandler { get; internal set; }
        /// <summary>
        /// Bot's reactions in Group Chats.
        /// </summary>
        internal ChatScanner? GroupChatUpdateHandler { get; set; }
        /// <summary>
        /// Bot's reactions in Supergroup Chats.
        /// </summary>
        internal ChatScanner? SupergroupChatUpdateHandler { get; set; }
        /// <summary>
        /// Bot's reactions in Channel Chats.
        /// </summary>
        internal ChatScanner? ChannelChatUpdateHandler { get; set; }

        /// <summary>
        /// Telegram's bot token.
        /// </summary>
        internal string? Token { private get; set; }
        /// <summary>
        /// Shows either bot's token is declared.
        /// </summary>
        public bool IsTokenDefined => Token != null;

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
        /// Bot's debug settings.
        /// </summary>
        public DebugSettings DebugSettings { get; private set; }
        #endregion

        #region Services
        /// <summary>
        /// Bot interior localizator.
        /// <para>
        /// <see cref="DefaultLocalizator"/> by default.
        /// </para>
        /// </summary>
        public ILocalizator Localizator { get; private set; }
        /// <summary>
        /// Logger used for debugging and informing developer/host.
        /// <para>
        /// <see cref="DefaultLocalizedLogger"/> by default.
        /// </para>
        /// </summary>
        public ILocalizedLogger LocalLogger { get; private set; }
        /// <summary>
        /// Delievery Service used for sending messages <see cref="IBuildableMessage"/> to server.
        /// <para>
        /// <see cref="DefaultDelieveryService"/> by default.
        /// </para>
        /// </summary>
        public IDelieveryService DelieveryService { get; set; }
        /// <summary>
        /// Simple IoC-Container for storing Singleton Services.
        /// </summary>
        private Dictionary<Type, object> Services { get; set; } = new();
        /// <summary>
        /// Adds a new service of a type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Interface type of a service</typeparam>
        /// <param name="service">Service to be stored</param>
        public void AddService<T>(T service) where T : notnull => Services.Add(typeof(T), service);
        /// <summary>
        /// Gets stored service of a type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Interface type of a service</typeparam>
        /// <returns>Service of the requested type.</returns>
        /// <exception cref="ServiceNotDefinedException">Thrown when <typeparamref name="T"/> is not defined.</exception>
        public object ResolveService<T>() where T : notnull
        {
            if (!Services.ContainsKey(typeof(T))) throw new ServiceNotDefinedException(typeof(T));
            return Services[typeof(T)];
        }
        #endregion

        internal BotManager(string token)
        {
            DebugSettings = DebugSettings.Default();
            Settings = new();

            Token = token;
            Bot = new TelegramBotClient(token);

            Localizator = new DefaultLocalizator("resources/locals");
            LocalLogger = new DefaultLocalizedLogger(Localizator);
            DelieveryService = new DefaultDelieveryService();
        }

        /// <summary>
        /// Reflectively compiles all <see cref="IOwnerCompilable"/> in the <see cref="BotManager"/> interior.
        /// Summoned during <see cref="BotManager"/> building.
        /// <para>See also: <seealso cref="BotBuilder.Build(string?)"/></para>
        /// </summary>
        public void ReflectiveCompile()
        {
            GetType()
                .GetProperties()
                .Where(x => x.GetValue(this) is IOwnerCompilable)
                .ToList()
                .ForEach(refCompile =>
                {
                    var cmpVal = refCompile.GetValue(this);
                    (cmpVal as IOwnerCompilable)!.ReflectiveCompile(cmpVal, this);
                });

            Services.Where(x => x.Value is IOwnerCompilable)
                .ToList()
                .ForEach(service => (service.Value as IOwnerCompilable)!.ReflectiveCompile(service, this));
        }

        /// <summary>
        /// Launches <see cref="ITelegramBotClient"/> <see cref="Bot"/> by starting server polling.
        /// </summary>
        /// <returns></returns>
        public async Task Listen()
        {
            var cts = new CancellationTokenSource();
            try
            {
                var me = await Bot.GetMeAsync();
                LocalLogger.LSuccess("system.StartUpMessage", format: me.Username);

                await Bot.ReceiveAsync(
                    new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                    cancellationToken: cts.Token);
            }
            catch (Exception exception)
            {
                LocalLogger.Log(exception);
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
        /// <returns></returns>
        private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            if (DebugSettings.ShouldPrintExceptions)
            {
                LocalLogger.Log(exception);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Unpacks and casts raw data, delegating raw server's <see cref="Update"/> to
        /// sub-handlers in <see cref="CastedUpdate"/> representation.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns></returns>
        /// <exception cref="BotManagerExcpetion"></exception>
        private async Task SubDelegateUpdate(Update update)
        {
            if (DebugSettings.ShouldPrintUpdates) LocalLogger.Log(update);

            try
            {
                long chatId = ChatIdByUpdate_Safe(update);
                ChatType senderChatType = ChatTypeByUpdate_Safe(update);
                ChatScanner? _handler = senderChatType switch
                {
                    ChatType.Private => PrivateChatUpdateHandler,
                    ChatType.Group => GroupChatUpdateHandler,
                    ChatType.Supergroup => SupergroupChatUpdateHandler,
                    ChatType.Channel => ChannelChatUpdateHandler,
                    _ => null,
                };

                if (_handler is null) throw new BotManagerExcpetion(DebugSettings.Nfy_ChatTypeNotSupported, "ChatTypeNotSupported", Enum.GetName(typeof(ChatType), senderChatType));

                await _handler.HandleUpdateAsync(new CastedUpdate(update, senderChatType, chatId));
            }
            catch (Exception exception)
            {
                if (DebugSettings.ShouldPrintExceptions)
                    LocalLogger.Log(exception);
            }
        }

        /// <summary>
        /// Tries to extracts <see cref="ChatType"/> from a raw server update otherwise
        /// throws <see cref="BotManagerExcpetion"/>.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Update's chat type.</returns>
        public ChatType ChatTypeByUpdate_Safe(Update update)
        {
            ChatType? senderChatType = ChatTypeByUpdate(update);
            if (senderChatType is null)
                throw new BotManagerExcpetion(DebugSettings.Nfy_ChatNotHandled, "ChatNotHandled");
            return senderChatType.Value;
        }
        /// <summary>
        /// Tries to extracts <see cref="ChatType"/> from a raw server update.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Update's chat type.</returns>
        public static ChatType? ChatTypeByUpdate(Update update)
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
        /// Tries to extracts chat's ID from a raw server update otherwise
        /// throws <see cref="BotManagerExcpetion"/>.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Update's chat ID.</returns>
        public long ChatIdByUpdate_Safe(Update update)
        {
            long? chatId = ChatIdByUpdate(update);
            if (chatId is null)
                throw new BotManagerExcpetion(DebugSettings.Nfy_ChatIdNotHandled, "ChatIdNotHandled");
            return chatId.Value;
        }
        /// <summary>
        /// Tries to extracts chat's ID from a raw server update.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Update's chat ID.</returns>
        public static long? ChatIdByUpdate(Update update)
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