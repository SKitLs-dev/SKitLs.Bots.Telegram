using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.external.Localizations;
using SKitLs.Bots.Telegram.Core.external.LocalizedLoggers;
using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
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
        /// Bot's reactions in Private Chats
        /// </summary>
        internal ChatScanner? PrivateChatUpdateHandler { get; set; }
        /// <summary>
        /// Bot's reactions in Group Chats
        /// </summary>
        internal ChatScanner? GroupChatUpdateHandler { get; set; }
        /// <summary>
        /// Bot's reactions in Supergroup Chats
        /// </summary>
        internal ChatScanner? SupergroupChatUpdateHandler { get; set; }
        /// <summary>
        /// Bot's reactions in Channel Chats
        /// </summary>
        internal ChatScanner? ChannelChatUpdateHandler { get; set; }

        /// <summary>
        /// Telegram's bot token
        /// </summary>
        internal string? Token { private get; set; }
        /// <summary>
        /// Shows either bot's token is declared.
        /// </summary>
        public bool IsTokenDefined => Token != null;

        /// <summary>
        /// External essential bot's instance
        /// </summary>
        public ITelegramBotClient Bot { get; private set; }
        #endregion

        /// <summary>
        /// Logger used for debugging and informing developer/host.
        /// </summary>
        public ILocalizedLogger LocalLogger { get; internal set; }

        internal BotManager(string token, ILocalizator localizator)
        {
            Token = token;

            Bot = new TelegramBotClient(token);

            LocalLogger = new DefaultLocalizedLogger(localizator);
        }

        /// <summary>
        /// Launches bot by starting server polling.
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
        /// Обрабатывает ошибку, полученную от бота
        /// </summary>
        /// <param name="client"></param>
        /// <param name="exception">Ошибка</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            if (TgApp.DebugSettings.ShouldPrintExceptions) LocalLogger.Log(exception);
            return Task.CompletedTask;
        }

        private async Task SubDelegateUpdate(Update update)
        {
            if (TgApp.DebugSettings.ShouldPrintUpdates) LocalLogger.Log(update);

            try
            {
                long? chatId = ChatIdByUpdate(update);
                ChatType? senderChatType = ChatTypeByUpdate(update);
                ChatScanner? _handler = senderChatType switch
                {
                    ChatType.Private => PrivateChatUpdateHandler,
                    ChatType.Group => GroupChatUpdateHandler,
                    ChatType.Supergroup => SupergroupChatUpdateHandler,
                    ChatType.Channel => ChannelChatUpdateHandler,
                    _ => null,
                };

                if (senderChatType is null)
                {
                    throw new BotManagerExcpetion(TgApp.DebugSettings.Nfy_ChatNotHandled, "ChatNotHandled");
                }
                if (_handler is null)
                {
                    throw new BotManagerExcpetion(TgApp.DebugSettings.Nfy_ChatTypeNotSupported, "ChatTypeNotSupported");
                }
                if (chatId is null)
                {
                    throw new BotManagerExcpetion(TgApp.DebugSettings.Nfy_ChatIdNotHandled, "ChatIdNotHandled");
                }

                await _handler.HandleUpdateAsync(
                    new CastedChatUpdate(update, Bot, chatId.Value, senderChatType.Value, LocalLogger));
            }
            catch (Exception exception)
            {
                if (TgApp.DebugSettings.ShouldPrintExceptions)
                    LocalLogger.Log(exception);
            }
        }

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
            else
                return null;
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
    }
}
