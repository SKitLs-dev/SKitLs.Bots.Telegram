using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// <see cref="ChatScanner"/> used for handling updates in different chats' types such as:
    /// Private, Group, Supergroup or Channel.
    /// Determines how bot should react on different triggers in defined chat type.
    /// Released in <see cref="BotManager"/>.
    /// </summary>
    internal class ChatScanner
    {
        /// <summary>
        /// Менеджер, осуществояющий управление авторизованными пользователями и связью с БД.
        /// Внешняя подключаемая служба.
        /// </summary>
        public IUsersManager? UsersManager { get; set; }

        /// <summary>
        /// Функция создания пользователя, на случай отключенного <see cref="UsersManager"/>.
        /// Необходимо для связи всех обновлений. Возвращённый пользователь обязательно должен определять
        /// <see cref="IBotUser.TelegramId"/> через входной аргумент типа long.
        /// По умолчанию возвращает <see cref="DefaultBotUser"/> с уровнем доступа
        /// <see cref="IBotUser.PermissionLevel"/> = -1.
        /// </summary>
        public Func<long, IBotUser> GetDefaultBotUser { get; set; }

        ///// <summary>
        ///// Действие, вызываемое при ошибке получения ID из обновления
        ///// </summary>
        //public DelegateAsyncTask? SenderIdExtractingError { get; set; }
        ///// <summary>
        ///// Действие, вызываемое в случае, если <see cref="UsersManager"/> или <see cref="GetDefaultBotUser"/>
        ///// вернули NULL. Вызывается перед ошибкой <see cref="InvalidOperationException"/>
        ///// </summary>
        //public IdNotPresentedAsync? IdNotPresentedError { get; set; }

        public IUpdateHandlerBase<SignedCallbackUpdate>? CallbackHandler { get; set; }
        public IUpdateHandlerBase<SignedMessageUpdate>? MessageHandler { get; set; }
        public IUpdateHandlerBase<SignedMessageUpdate>? EditedMessageHandler { get; set; }
        public IUpdateHandlerBase<AnonimMessageUpdate>? ChannelPostHandler { get; set; }
        public IUpdateHandlerBase<AnonimMessageUpdate>? EditedChannelPostHandler { get; set; }

        public IUpdateHandlerBase<CastedUpdate>? ChatJoinRequestHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? ChatMemberHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? ChosenInlineResultHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? InlineQueryHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? MyChatMemberHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? PollHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? PollAnswerHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? PreCheckoutQueryHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? ShippingQueryHandler { get; set; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ChatScanner()
        {
            GetDefaultBotUser = (id) => new DefaultBotUser(id);
        }

        /// <summary>
        /// Метод-обработчик входящего события, проверяющий пользователя-отправителя и делигирующий 
        /// пересобранное обновление на один из обработчиков в зависимости от типа обновления.
        /// </summary>
        /// <param name="update">Входящее обновление</param>
        public async Task HandleUpdateAsync(CastedUpdate update)
        {
            IBotUser? sender = null;
            long? id = GetSenderId(update.OriginalSource);
            if (id != null)
            {
                if (UsersManager is not null)
                {
                    if (await UsersManager.IsUserRegistered(id.Value))
                        sender = await UsersManager.GetUserById(id.Value);
                    else
                        sender = await UsersManager.ProccessNewUser(id.Value);
                }
                else sender = GetDefaultBotUser(id.Value);

                if (sender == null)
                {
                    //if (IdNotPresentedError != null)
                    //    await IdNotPresentedError(id.Value);
                }
            }
            else
            {
                //if (SenderIdExtractingError != null)
                //    await SenderIdExtractingError();
            }

            IUpdateHandlerBase? suitHandler = update.Type switch
            {
                UpdateType.CallbackQuery => CallbackHandler,
                UpdateType.ChannelPost => ChannelPostHandler,
                UpdateType.ChatJoinRequest => ChatJoinRequestHandler,
                UpdateType.ChatMember => ChatMemberHandler,
                UpdateType.ChosenInlineResult => ChosenInlineResultHandler,
                UpdateType.EditedChannelPost => EditedChannelPostHandler,
                UpdateType.EditedMessage => EditedMessageHandler,
                UpdateType.InlineQuery => InlineQueryHandler,
                UpdateType.Message => MessageHandler,
                UpdateType.MyChatMember => MyChatMemberHandler,
                UpdateType.Poll => PollHandler,
                UpdateType.PollAnswer => PollAnswerHandler,
                UpdateType.PreCheckoutQuery => PreCheckoutQueryHandler,
                UpdateType.ShippingQuery => ShippingQueryHandler,
                _ => null
            };

            if (suitHandler is not null)
            {
                await suitHandler.HandleUpdateAsync(update, sender);
            }
            if (sender is not null && UsersManager is not null && UsersManager.SignedEventHandled is not null)
                await UsersManager.SignedEventHandled.Invoke(sender);
        }

        /// <summary>
        /// Определяет ID пользователя на основе входящего обновления, если обновление исходит от
        /// определённого пользователя.
        /// </summary>
        /// <param name="update">Входящее обновление</param>
        /// <returns>ID поьзователя или NULL в случае анонимного обновления.</returns>
        public static long? GetSenderId(Update update)
        {
            if (update.CallbackQuery != null && update.CallbackQuery.Message != null)
                return update.CallbackQuery.From?.Id;
            else if (update.ChannelPost != null)
                return update.ChannelPost.From?.Id;
            else if (update.ChatJoinRequest != null)
                return update.ChatJoinRequest.From?.Id;
            else if (update.ChatMember != null)
                return update.ChatMember.From?.Id;
            else if (update.EditedChannelPost != null)
                return update.EditedChannelPost.From?.Id;
            else if (update.EditedMessage != null)
                return update.EditedMessage.From?.Id;
            else if (update.Message != null)
                return update.Message.From?.Id;
            else if (update.MyChatMember != null)
                return update.MyChatMember.From?.Id;
            else return null;
        }
    }
}