using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Model.Users;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.Building
{
    /// <summary>
    /// Represents the entry point for creating chat handlers. This class serves as a wizard constructor for the <see cref="ChatScanner"/> class.
    /// Each chat handles updates of a certain chat type (<see cref="ChatType"/>).
    /// </summary>
    public class ChatDesigner
    {
        private readonly ChatScanner _chatScanner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatDesigner"/> class.
        /// </summary>
        private ChatDesigner() => _chatScanner = new ChatScanner();

        /// <summary>
        /// Creates a new instance of the <see cref="ChatDesigner"/> class.
        /// </summary>
        /// <returns>A new instance of <see cref="ChatDesigner"/>.</returns>
        public static ChatDesigner NewDesigner() => new();

        #region User Settings

        /// <summary>
        /// Overrides the default user function (<see cref="ChatScanner.GetDefaultBotUser"/>). This function creates a new default user to ensure all <see cref="ISignedUpdate"/> are handled properly.
        /// </summary>
        /// <param name="func">The function to be implemented.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner OverrideDefaultUserFunc(Func<long, IBotUser> func)
        {
            _chatScanner.GetDefaultBotUser = func;
            return this;
        }

        /// <summary>
        /// Sets a custom <see cref="IUsersManager"/> for this chat. The users manager verifies user authorization.
        /// <para>When <paramref name="manager"/> is <see langword="null"/>, <see cref="ChatScanner.GetDefaultBotUser"/> is used. You can access it via <see cref="OverrideDefaultUserFunc(Func{long, IBotUser})"/>.</para>
        /// </summary>
        /// <param name="manager">The users manager to be set.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseUsersManager(IUsersManager manager)
        {
            _chatScanner.UsersManager = manager;
            return this;
        }

        #endregion

        #region HandlersUpdate

        /// <summary>
        /// Sets the message handler (<see cref="ChatScanner.MessageHandler"/>).
        /// </summary>
        /// <param name="handler">The new message handler.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseMessageHandler(IUpdateHandlerBase<SignedMessageUpdate>? handler)
        {
            _chatScanner.MessageHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the edited message handler (<see cref="ChatScanner.EditedMessageHandler"/>).
        /// </summary>
        /// <param name="handler">The new edited message handler.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseEditedMessageHandler(IUpdateHandlerBase<SignedMessageUpdate>? handler)
        {
            _chatScanner.EditedMessageHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the channel post handler (<see cref="ChatScanner.ChannelPostHandler"/>).
        /// </summary>
        /// <param name="handler">The new channel post handler.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseChannelPostHandler(IUpdateHandlerBase<AnonymMessageUpdate>? handler)
        {
            _chatScanner.ChannelPostHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the edited channel post handler (<see cref="ChatScanner.EditedChannelPostHandler"/>).
        /// </summary>
        /// <param name="handler">The new edited channel post handler.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseEditedChannelPostHandler(IUpdateHandlerBase<AnonymMessageUpdate>? handler)
        {
            _chatScanner.EditedChannelPostHandler = handler;
            return this;
        }
        /// <summary>
        /// Sets the callback handler (<see cref="ChatScanner.CallbackHandler"/>).
        /// </summary>
        /// <param name="handler">The new callback handler.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseCallbackHandler(IUpdateHandlerBase<SignedCallbackUpdate>? handler)
        {
            _chatScanner.CallbackHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling chat join requests.
        /// </summary>
        /// <param name="handler">The handler for chat join requests.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseChatJoinRequestHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.ChatJoinRequestHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling chat members.
        /// </summary>
        /// <param name="handler">The handler for chat members.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseChatMemberHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.ChatMemberHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling chosen inline results.
        /// </summary>
        /// <param name="handler">The handler for chosen inline results.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseChosenInlineResultHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.ChosenInlineResultHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling inline queries.
        /// </summary>
        /// <param name="handler">The handler for inline queries.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseInlineQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.InlineQueryHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling updates about the bot being added to or removed from a group chat.
        /// </summary>
        /// <param name="handler">The handler for bot's chat member updates.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseMyChatMemberHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.MyChatMemberHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling polls.
        /// </summary>
        /// <param name="handler">The handler for polls.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UsePollHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.PollHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling poll answers.
        /// </summary>
        /// <param name="handler">The handler for poll answers.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UsePollAnswerHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.PollAnswerHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling pre-checkout queries.
        /// </summary>
        /// <param name="handler">The handler for pre-checkout queries.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UsePreCheckoutQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.PreCheckoutQueryHandler = handler;
            return this;
        }

        /// <summary>
        /// Sets the handler for handling shipping queries.
        /// </summary>
        /// <param name="handler">The handler for shipping queries.</param>
        /// <returns>The updated instance of <see cref="ChatDesigner"/>.</returns>
        public ChatDesigner UseShippingQueryHandler(IUpdateHandlerBase<CastedUpdate>? handler)
        {
            _chatScanner.ShippingQueryHandler = handler;
            return this;
        }
        #endregion

        /// <summary>
        /// Compiles the created instance and returns the built one.
        /// </summary>
        /// <param name="debugName">The custom debug name (<see cref="BotManager.DebugName"/>).</param>
        /// <returns>The compiled instance of <see cref="ChatScanner"/>.</returns>
        internal ChatScanner Build(string? debugName = null)
        {
            if (debugName is not null)
                _chatScanner.DebugName = debugName;
            return _chatScanner;
        }
    }
}
