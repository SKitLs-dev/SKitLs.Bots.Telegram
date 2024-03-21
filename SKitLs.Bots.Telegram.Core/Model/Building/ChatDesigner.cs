using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.Building
{
    // XML-Doc Update
    /// <summary>
    /// Chat Handler creating process enter point. <see cref="ChatScanner"/> class wizard constructor.
    /// Each chat handles update in a certain chat type <see cref="ChatType"/>.
    /// </summary>
    public class ChatDesigner
    {
        /// <summary>
        /// Constructing instance.
        /// </summary>
        private readonly ChatScanner _chatScanner;
        /// <summary>
        /// Creates a new instance of the wizard constructor.
        /// </summary>
        private ChatDesigner() => _chatScanner = new ChatScanner();
        /// <summary>
        /// Creates a new instance of the wizard constructor.
        /// </summary>
        public static ChatDesigner NewDesigner() => new();

        /// <summary>
        /// Sets custom <see cref="IUsersManager"/> <see cref="ChatScanner.UsersManager"/> for this
        /// chat. Users manager verifies users authorization.
        /// <para>
        /// When <see langword="null"/>, <see cref="ChatScanner.GetDefaultBotUser"/> is used.
        /// Access it via <see cref="OverrideDefaultUserFunc(Func{long, IBotUser})"/>
        /// </para>
        /// </summary>
        /// <param name="manager">Manager to be implemented.</param>
        /// <returns>Updated instance.</returns>
        public ChatDesigner UseUsersManager(IUsersManager manager)
        {
            _chatScanner.UsersManager = manager;
            return this;
        }
        #region Common Settings
        /// <summary>
        /// Overrides default user function (<see cref="ChatScanner.GetDefaultBotUser"/>).
        /// This function creates new default user to be sure all <see cref="ISignedUpdate"/> are
        /// handled properly.
        /// </summary>
        /// <param name="func">Func to be implemented.</param>
        /// <returns>Updated instance.</returns>
        public ChatDesigner OverrideDefaultUserFunc(Func<long, IBotUser> func)
        {
            _chatScanner.GetDefaultBotUser = func;
            return this;
        }
        #endregion

        #region HandlersUpdate
        /// <summary>
        /// Sets <see cref="ChatScanner.MessageHandler"/>.
        /// </summary>
        /// <param name="handler">New handler.</param>
        public ChatDesigner UseMessageHandler(IUpdateHandlerBase<SignedMessageUpdate>? handler)
        {
            _chatScanner.MessageHandler = handler;
            return this;
        }
        /// <summary>
        /// Sets <see cref="ChatScanner.EditedMessageHandler"/>.
        /// </summary>
        /// <param name="handler">New handler.</param>
        public ChatDesigner UseEditedMessageHandler(IUpdateHandlerBase<SignedMessageUpdate>? handler)
        {
            _chatScanner.EditedMessageHandler = handler;
            return this;
        }
        /// <summary>
        /// Sets <see cref="ChatScanner.ChannelPostHandler"/>.
        /// </summary>
        /// <param name="handler">New handler.</param>
        public ChatDesigner UseChannelPostHandler(IUpdateHandlerBase<AnonymMessageUpdate>? handler)
        {
            _chatScanner.ChannelPostHandler = handler;
            return this;
        }
        /// <summary>
        /// Sets <see cref="ChatScanner.EditedChannelPostHandler"/>.
        /// </summary>
        /// <param name="handler">New handler.</param>
        public ChatDesigner UseEditedChannelPostHandler(IUpdateHandlerBase<AnonymMessageUpdate>? handler)
        {
            _chatScanner.EditedChannelPostHandler = handler;
            return this;
        }
        /// <summary>
        /// Sets <see cref="ChatScanner.CallbackHandler"/>.
        /// </summary>
        /// <param name="handler">New handler.</param>
        public ChatDesigner UseCallbackHandler(IUpdateHandlerBase<SignedCallbackUpdate>? handler)
        {
            _chatScanner.CallbackHandler = handler;
            return this;
        }

        public ChatDesigner UseChatJoinRequestHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.ChatJoinRequestHandler = handler;
            return this;
        }
        public ChatDesigner UseChatMemberHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.ChatMemberHandler = handler;
            return this;
        }
        public ChatDesigner UseChosenInlineResultHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.ChosenInlineResultHandler = handler;
            return this;
        }
        public ChatDesigner UseInlineQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.InlineQueryHandler = handler;
            return this;
        }
        public ChatDesigner UseMyChatMemberHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.MyChatMemberHandler = handler;
            return this;
        }
        public ChatDesigner UsePollHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.PollHandler = handler;
            return this;
        }
        public ChatDesigner UsePollAnswerHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.PollAnswerHandler = handler;
            return this;
        }
        public ChatDesigner UsePreCheckoutQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.PreCheckoutQueryHandler = handler;
            return this;
        }
        public ChatDesigner UseShippingQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.ShippingQueryHandler = handler;
            return this;
        }
        #endregion

        /// <summary>
        /// Compiles created instance and returns the built one.
        /// </summary>
        /// <param name="debugName">Custom debug name (<see cref="BotManager.DebugName"/>).</param>
        internal ChatScanner Build(string? debugName = null)
        {
            _chatScanner.DebugName = debugName ?? nameof(ChatScanner);
            return _chatScanner;
        }
    }
}