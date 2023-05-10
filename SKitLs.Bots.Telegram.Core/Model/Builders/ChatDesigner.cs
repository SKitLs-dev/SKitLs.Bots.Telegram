using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;

// TODO
namespace SKitLs.Bots.Telegram.Core.Model.Builders
{
    /// <summary>
    /// Chat Handler creating proccess enter point. <see cref="ChatScanner"/> class wizard constructor.
    /// Each chat handles update in a certain chat type <see cref="ChatType"/>.
    /// </summary>
    public class ChatDesigner
    {
        /// <summary>
        /// Constructing instance.
        /// </summary>
        private readonly ChatScanner _updateHandler;
        /// <summary>
        /// Creates a new instance of the wizard constructor.
        /// </summary>
        private ChatDesigner() => _updateHandler = new ChatScanner();
        /// <summary>
        /// Creates a new instance of the wizard constructor.
        /// </summary>
        public static ChatDesigner NewDesigner() => new();

        /// <summary>
        /// Устанавливает для чата <see cref="TypedChatUpdateHandler.UsersManager"/>, осуществляющего
        /// проверку и регистрацию пользователей. По умолчанию NULL.
        /// <para>Когда NULL (хранение данных пользователей не требуется) обработка передаётся
        /// <see cref="TypedChatUpdateHandler.GetDefaultBotUser"/></para>
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public ChatDesigner UseUsersManager(IUsersManager manager)
        {
            _updateHandler.UsersManager = manager;
            return this;
        }
        #region Common Settings
        /// <summary>
        /// Переопределяет функцию обработки пользователей <see cref="TypedChatUpdateHandler.GetDefaultBotUser"/>
        /// на случай <see cref="TypedChatUpdateHandler.UsersManager"/> = NULL.
        /// Возвращаемый <see cref="IBotUser"/> должен определять <see cref="IBotUser.TelegramId"/> как входной
        /// параметр long и не быть равен NULL. По умолчанию возвращает <see cref="DefaultBotUser"/> с уровнем
        /// доступа <see cref="IBotUser.PermissionLevel"/> = -1.
        /// </summary>
        /// <param name="func">Функция создания временного пользователя на основе его телеграм-ID</param>
        /// <returns></returns>
        public ChatDesigner OverrideDefaultUserFunc(Func<long, IBotUser> func)
        {
            _updateHandler.GetDefaultBotUser = func;
            return this;
        }
        ///// <summary>
        ///// Определеяет <see cref="TypedChatUpdateHandler.SenderIdExtractingError"/>. По умолчанию NULL.
        ///// </summary>
        ///// <param name="handler"></param>
        ///// <returns></returns>
        //public ChatBehaviorBuilder AddSenderIdExtractingErrorHandler(DelegateAsyncTask handler)
        //{
        //    _updateHandler.SenderIdExtractingError = handler;
        //    return this;
        //}
        ///// <summary>
        ///// Определяет <see cref="TypedChatUpdateHandler.IdNotPresentedError"/>. По умолчанию NULL.
        ///// </summary>
        ///// <param name="handler"></param>
        ///// <returns></returns>
        //public ChatBehaviorBuilder AddIdNotPresentedErrorHandler(IdNotPresentedAsync handler)
        //{
        //    _updateHandler.IdNotPresentedError = handler;
        //    return this;
        //}
        #endregion

        #region HandlersUpdate
        /// <summary>
        /// Изменяет <see cref="TypedChatUpdateHandler.MessageHandler"/>
        /// </summary>
        /// <param name="handler">Новый обработчик</param>
        public ChatDesigner UseMessageHandler(IUpdateHandlerBase<SignedMessageUpdate>? handler)
        {
            _updateHandler.MessageHandler = handler;
            return this;
        }
        /// <summary>
        /// Изменяет <see cref="ChatScanner.EditedMessageHandler"/>
        /// </summary>
        /// <param name="handler">Новый обработчик</param>
        public ChatDesigner UseEditedMessageHandler(IUpdateHandlerBase<SignedMessageUpdate>? handler)
        {
            _updateHandler.EditedMessageHandler = handler;
            return this;
        }
        /// <summary>
        /// Изменяет <see cref="ChatScanner.ChannelPostHandler"/>
        /// </summary>
        /// <param name="handler">Новый обработчик</param>
        public ChatDesigner UseChannelPostHandler(IUpdateHandlerBase<AnonimMessageUpdate>? handler)
        {
            _updateHandler.ChannelPostHandler = handler;
            return this;
        }
        /// <summary>
        /// Изменяет <see cref="ChatScanner.EditedChannelPostHandler"/>
        /// </summary>
        /// <param name="handler">Новый обработчик</param>
        public ChatDesigner UseEditedChannelPostHandler(IUpdateHandlerBase<AnonimMessageUpdate>? handler)
        {
            _updateHandler.EditedChannelPostHandler = handler;
            return this;
        }
        /// <summary>
        /// Изменяет <see cref="TypedChatUpdateHandler.CallbackManager"/>
        /// </summary>
        /// <param name="handler">Новый менеджер</param>
        public ChatDesigner UseCallbackHandler(IUpdateHandlerBase<SignedCallbackUpdate>? handler)
        {
            _updateHandler.CallbackHandler = handler;
            return this;
        }

        public ChatDesigner UseChatJoinRequestHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.ChatJoinRequestHandler = handler;
            return this;
        }
        public ChatDesigner UseChatMemberHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.ChatMemberHandler = handler;
            return this;
        }
        public ChatDesigner UseChosenInlineResultHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.ChosenInlineResultHandler = handler;
            return this;
        }
        public ChatDesigner UseInlineQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.InlineQueryHandler = handler;
            return this;
        }
        public ChatDesigner UseMyChatMemberHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.MyChatMemberHandler = handler;
            return this;
        }
        public ChatDesigner UsePollHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.PollHandler = handler;
            return this;
        }
        public ChatDesigner UsePollAnswerHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.PollAnswerHandler = handler;
            return this;
        }
        public ChatDesigner UsePreCheckoutQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.PreCheckoutQueryHandler = handler;
            return this;
        }
        public ChatDesigner UseShippingQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _updateHandler.ShippingQueryHandler = handler;
            return this;
        }
        #endregion

        #region Messages Rules
        ///// <summary>
        ///// Изменяет <see cref="IAnonMessageUpdateHandler.TextMessageUpdateHandler"/> для
        ///// <see cref="TypedChatUpdateHandler.MessageHandler"/>.
        ///// По умолчанию: <see cref="DefaultTextMessageUpdateHandler"/>
        ///// </summary>
        ///// <param name="handler">Новый обработчик текстовых сообщений</param>
        //public ChatDesigner UseCustomTextMessageUpdateHandler(ITextMessageUpdateHandler handler)
        //{
        //    _updateHandler.MessageHandler.TextMessageUpdateHandler = handler;
        //    return this;
        //}
        ///// <summary>
        ///// Изменяет <see cref="ITextMessageUpdateHandler.IsCommand"/>
        ///// для <see cref="TypedChatUpdateHandler.MessageHandler"/> =>
        ///// <see cref="IAnonMessageUpdateHandler.TextMessageUpdateHandler"/>.
        ///// По умолчанию: начинается с '/'
        ///// </summary>
        ///// <param name="checker">Правило проверки</param>
        //public ChatDesigner UseCustomCommandChecker(Func<string, bool> checker)
        //{
        //    _updateHandler.MessageHandler.TextMessageUpdateHandler.IsCommand = checker;
        //    return this;
        //}
        ///// <summary>
        ///// Изменяет <see cref="ITextMessageUpdateHandler.CommandsManager"/>
        ///// для <see cref="TypedChatUpdateHandler.MessageHandler"/> =>
        ///// <see cref="IAnonMessageUpdateHandler.TextMessageUpdateHandler"/>.
        ///// По умолчанию: <see cref="DefaultCommandsManager"/>
        ///// </summary>
        ///// <param name="manager">Новый менеджер</param>
        //public ChatDesigner UseCustomCommandsManager(ICommandsManager manager)
        //{
        //    _updateHandler.MessageHandler.TextMessageUpdateHandler.CommandsManager = manager;
        //    return this;
        //}
        ///// <summary>
        ///// Изменяет <see cref="ITextMessageUpdateHandler.TextInputManager"/>
        ///// для <see cref="TypedChatUpdateHandler.MessageHandler"/> =>
        ///// <see cref="IAnonMessageUpdateHandler.TextMessageUpdateHandler"/>.
        ///// По умолчанию: <see cref="DefaultTextInputManager"/>
        ///// </summary>
        ///// <param name="manager">Новый менеджер</param>
        ///// <returns></returns>
        //public ChatDesigner UseCustomTextInputManager(ITextInputManager manager)
        //{
        //    _updateHandler.MessageHandler.TextMessageUpdateHandler.TextInputManager = manager;
        //    return this;
        //}
        #endregion

        //#region Behavior Rules
        ///// <summary>
        ///// Добавляет секцию состояния комманд для <see cref="TypedChatUpdateHandler.MessageHandler"/> =>
        ///// <see cref="IMessageUpdateHandler.TextMessageUpdateHandler"/> =>
        ///// <see cref="ITextMessageUpdateHandler.CommandsManager"/>.
        ///// </summary>
        ///// <param name="section">Секция состояния</param>
        //public ChatDesigner AddCommandsStateSection(CommandsStateSection section)
        //{
        //    _updateHandler.MessageHandler.TextMessageUpdateHandler.CommandsManager
        //        .AddCommandsStateSection(section);
        //    return this;
        //}
        ///// <summary>
        ///// Добавляет секцию состояния ввода для <see cref="TypedChatUpdateHandler.MessageHandler"/> =>
        ///// <see cref="IMessageUpdateHandler.TextMessageUpdateHandler"/> =>
        ///// <see cref="ITextMessageUpdateHandler.TextInputManager"/>.
        ///// </summary>
        ///// <param name="section">Секция состояния</param>
        //public ChatDesigner AddInputStateSection(InputStateSection section)
        //{
        //    _updateHandler.MessageHandler.TextMessageUpdateHandler.TextInputManager
        //        .AddInputStateSection(section);
        //    return this;
        //}
        ///// <summary>
        ///// Добавляет секцию состояния коллбэков для <see cref="TypedChatUpdateHandler.CallbackManager"/>
        ///// </summary>
        ///// <param name="section">Секция состояния</param>
        //public ChatDesigner AddCallbacksStateSection(CallbacksStateSection section)
        //{
        //    _updateHandler.CallbackManager.AddCallbacksStateSection(section);
        //    return this;
        //}

        //public ChatDesigner AddCallbackStateDefaultAnswer(DefaultStateAnswer answer)
        //{
        //    _updateHandler.CallbackManager.AddStateDefaultAnswer(answer);
        //    return this;
        //}
        //#endregion

        ///// <summary>
        ///// Возвращает собранные правила обработки обновлений
        ///// </summary>
        ///// <returns></returns>
        internal ChatScanner Build() => _updateHandler;
    }
}
