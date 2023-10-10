using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using Telegram.Bot.Types;
using TEnum = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
    // XML-Doc Update
    /// <summary>
    /// <see cref="ChatScanner"/> used for handling updates in different chats' types (<see cref="TEnum.ChatType"/>)
    /// such as: Private, Group, Supergroup or Channel.
    /// Determines how bot should react on different triggers in defined chat type.
    /// Released in <see cref="BotManager"/>.
    /// <para>
    /// Second architecture level.
    /// Upper: <see cref="BotManager"/>.
    /// Lower: <see cref="IUpdateHandlerBase"/>.
    /// </para>
    /// <para>Supports: <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/></para>
    /// </summary>
    public sealed class ChatScanner : IDebugNamed, IOwnerCompilable, IActionsHolder
    {
        #region Properties
        private string? _debugName;
        /// <summary>
        /// Name, used for simplifying debugging process.
        /// </summary>
        public string DebugName
        {
            get => _debugName ?? Enum.GetName(typeof(TEnum.ChatType), ChatType) ?? "Unknown";
            set => _debugName = value;
        }

        private BotManager? _owner;
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <summary>
        /// Specified method that raised during reflective <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/> compilation.
        /// Declare it to extend preset functionality.
        /// Invoked after <see cref="Owner"/> updating, but before recursive update.
        /// </summary>
        public Action<object, BotManager>? OnCompilation => null;
        
        /// <summary>
        /// Type of a chat that current scanner is bind to.
        /// </summary>
        public TEnum.ChatType ChatType { get; internal set; }
        #endregion

        #region Users Handlers
        /// <summary>
        /// Custom service used to manage authorized users. Can be released with vanilla roles and
        /// permissions schema. Used to connect internal mechanisms with external DataBases.
        /// Doesn't have default realization.
        /// </summary>
        public IUsersManager? UsersManager { get; internal set; }

        /// <summary>
        /// Default function used to handle updates and pass <see cref="IBotUser"/> instance to next
        /// level. Gets Telegram Id as an argument and returns an instance of <see cref="DefaultBotUser"/>
        /// by default.
        /// <para>
        /// Used only in case <c><see cref="UsersManager"/> = null</c> or manager's internal problems.
        /// </para>
        /// </summary>
        public Func<long, IBotUser> GetDefaultBotUser { get; internal set; }
        #endregion

        // TODO: Make generic via Enum.GetValues() of UpdateType enum.
        #region Update Handlers
        public IUpdateHandlerBase<SignedCallbackUpdate>? CallbackHandler { get; set; }
        public IUpdateHandlerBase<SignedMessageUpdate>? MessageHandler { get; set; }
        public IUpdateHandlerBase<SignedMessageUpdate>? EditedMessageHandler { get; set; }
        public IUpdateHandlerBase<AnonymMessageUpdate>? ChannelPostHandler { get; set; }
        public IUpdateHandlerBase<AnonymMessageUpdate>? EditedChannelPostHandler { get; set; }

        public IUpdateHandlerBase<CastedUpdate>? ChatJoinRequestHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? ChatMemberHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? ChosenInlineResultHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? InlineQueryHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? MyChatMemberHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? PollHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? PollAnswerHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? PreCheckoutQueryHandler { get; set; }
        public IUpdateHandlerBase<CastedUpdate>? ShippingQueryHandler { get; set; }
        #endregion

        internal ChatScanner()
        {
            GetDefaultBotUser = (id) => new DefaultBotUser(id);
        }
        
        /// <summary>
        /// Collects all <see cref="IBotAction"/>s declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public List<IBotAction> GetActionsContent()
        {
            var res = new List<IBotAction>();
            GetDeclaredHandlers().ForEach(x => res.AddRange(x.GetActionsContent()));
            return res;
        }

        /// <summary>
        /// Incoming <see cref="ICastedUpdate"/> handler. Verifies or builds sender, casts update
        /// and subdelegates it to one of specific sub handlers.
        /// <para>
        /// Can be useful: <c><seealso cref="GetDeclaredHandlers"/></c>
        /// </para>
        /// </summary>
        /// <param name="update">Incoming update.</param>
        /// <exception cref="NullSenderException"></exception>
        /// <exception cref="BotManagerException"></exception>
        public async Task HandleUpdateAsync(ICastedUpdate update)
        {
            IBotUser? sender = null;
            long id = GetSenderId(update.OriginalSource);
            if (UsersManager is not null)
            {
                if (await UsersManager.IsUserRegisteredAsync(id))
                    sender = await UsersManager.GetUserByIdAsync(id);
                else
                    sender = await UsersManager.RegisterNewUserAsync(update);
            }
            else sender = GetDefaultBotUser(id);

            if (sender is null)
                throw new NullSenderException(this);

            IUpdateHandlerBase? suitHandler = update.Type switch
            {
                TEnum.UpdateType.CallbackQuery => CallbackHandler,
                TEnum.UpdateType.ChannelPost => ChannelPostHandler,
                TEnum.UpdateType.ChatJoinRequest => ChatJoinRequestHandler,
                TEnum.UpdateType.ChatMember => ChatMemberHandler,
                TEnum.UpdateType.ChosenInlineResult => ChosenInlineResultHandler,
                TEnum.UpdateType.EditedChannelPost => EditedChannelPostHandler,
                TEnum.UpdateType.EditedMessage => EditedMessageHandler,
                TEnum.UpdateType.InlineQuery => InlineQueryHandler,
                TEnum.UpdateType.Message => MessageHandler,
                TEnum.UpdateType.MyChatMember => MyChatMemberHandler,
                TEnum.UpdateType.Poll => PollHandler,
                TEnum.UpdateType.PollAnswer => PollAnswerHandler,
                TEnum.UpdateType.PreCheckoutQuery => PreCheckoutQueryHandler,
                TEnum.UpdateType.ShippingQuery => ShippingQueryHandler,
                _ => null
            };

            if (suitHandler is null)
                throw new SKTgException("cs.NullUpdateHandler", SKTEOriginType.Inexternal, ToString(), Enum.GetName(typeof(TEnum.UpdateType), update.Type));
            
            await suitHandler.HandleUpdateAsync(update, sender);
            
            if (sender is not null && UsersManager is not null && UsersManager.SignedUpdateHandled is not null)
                await UsersManager.SignedUpdateHandled.Invoke(sender, update);
        }

        /// <summary>
        /// Extracts sender's ID from a raw server update or
        /// throws <see cref="BotManagerException"/> otherwise.
        /// </summary>
        /// <param name="update">Original server update.</param>
        /// <returns>Sender's ID.</returns>
        /// <exception cref="BotManagerException"></exception>
        public long GetSenderId(Update update) => TryGetSenderId(update)
            ?? throw new BotManagerException("cs.UserIdExtractError", this, update.Id.ToString());
        /// <summary>
        /// Tries to extract sender's ID from a raw server update.
        /// </summary>
        /// <param name="update">Original server update.</param>
        /// <returns>Nullable sender's ID.</returns>
        public static long? TryGetSenderId(Update update)
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

        /// <summary>
        /// Extracts sender instance of a type <see cref="User"/> from a raw server update or
        /// throws <see cref="BotManagerException"/> otherwise.
        /// </summary>
        /// <param name="update">Original server update.</param>
        /// <returns>Not-null instance of a sender.</returns>
        /// <exception cref="BotManagerException"></exception>
        public User GetSender(Update update) => GetSender(update, this);
        /// <summary>
        /// Extracts sender instance of a type <see cref="User"/> from a raw server update or
        /// throws <see cref="BotManagerException"/> otherwise.
        /// </summary>
        /// <param name="update">Original server update.</param>
        /// <param name="requester">An object that has requested extraction.</param>
        /// <returns>Not-null instance of a sender.</returns>
        /// <exception cref="BotManagerException"></exception>
        public static User GetSender(Update update, object requester) => TryGetSender(update)
            ?? throw new BotManagerException("cs.UserExtractError", requester, update.Id.ToString());
        /// <summary>
        /// Tries to extract sender instance of a type <see cref="User"/> from a raw server update.
        /// </summary>
        /// <param name="update">Original server update.</param>
        /// <returns>Nullable instance of a sender.</returns>
        public static User? TryGetSender(Update update)
        {
            if (update.CallbackQuery != null && update.CallbackQuery.Message != null)
                return update.CallbackQuery.From;
            else if (update.ChannelPost != null)
                return update.ChannelPost.From;
            else if (update.ChatJoinRequest != null)
                return update.ChatJoinRequest.From;
            else if (update.ChatMember != null)
                return update.ChatMember.From;
            else if (update.EditedChannelPost != null)
                return update.EditedChannelPost.From;
            else if (update.EditedMessage != null)
                return update.EditedMessage.From;
            else if (update.Message != null)
                return update.Message.From;
            else if (update.MyChatMember != null)
                return update.MyChatMember.From;
            else return null;
        }
        /// <summary>
        /// Collects all defined not null handlers in one collection.
        /// </summary>
        /// <returns>Collection of defined handlers.</returns>
        public List<IUpdateHandlerBase> GetDeclaredHandlers()
        {
            var res = new List<IUpdateHandlerBase>();
            if (CallbackHandler is not null)
                res.Add(CallbackHandler);
            if (MessageHandler is not null)
                res.Add(MessageHandler);
            if (EditedMessageHandler is not null)
                res.Add(EditedMessageHandler);
            if (ChannelPostHandler is not null)
                res.Add(ChannelPostHandler);
            if (EditedChannelPostHandler is not null)
                res.Add(EditedChannelPostHandler);

            if (ChatJoinRequestHandler is not null)
                res.Add(ChatJoinRequestHandler);
            if (ChatMemberHandler is not null)
                res.Add(ChatMemberHandler);
            if (ChosenInlineResultHandler is not null)
                res.Add(ChosenInlineResultHandler);
            if (InlineQueryHandler is not null)
                res.Add(InlineQueryHandler);
            if (MyChatMemberHandler is not null)
                res.Add(MyChatMemberHandler);
            if (PollHandler is not null)
                res.Add(PollHandler);
            if (PollAnswerHandler is not null)
                res.Add(PollAnswerHandler);
            if (PreCheckoutQueryHandler is not null)
                res.Add(PreCheckoutQueryHandler);
            if (ShippingQueryHandler is not null)
                res.Add(ShippingQueryHandler);
            return res;
        }

        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override string? ToString() => $"{DebugName} ({Owner})";
    }
}