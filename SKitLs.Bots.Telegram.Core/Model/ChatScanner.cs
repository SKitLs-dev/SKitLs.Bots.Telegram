using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types;
using TEnum = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
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
    public class ChatScanner : IDebugNamed, IOwnerCompilable, IActionsHolder
    {
        #region Properties
        public string? DebugName { get; set; }

        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;
        
        /// <summary>
        /// Type of a chat that current scanner is binded to.
        /// </summary>
        public TEnum.ChatType ChatType { get; internal set; }
        #endregion

        #region Users Handlers
        /// <summary>
        /// Custom service used to manage authorized users. Can be released with vanilla roles and
        /// permissions schema. Used to connect internal mechanisms with external DataBases.
        /// Doesn't have default realisation.
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
        #endregion

        internal ChatScanner()
        {
            GetDefaultBotUser = (id) => new DefaultBotUser(id);
        }
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
        /// <param name="update">Incoming update</param>
        /// <exception cref="NullSenderException"></exception>
        /// <exception cref="BotManagerExcpetion"></exception>
        public async Task HandleUpdateAsync(ICastedUpdate update)
        {
            IBotUser? sender = null;
            long id = GetSenderId(update.OriginalSource);
            if (UsersManager is not null)
            {
                if (await UsersManager.IsUserRegistered(id))
                    sender = await UsersManager.GetUserById(id);
                else
                    sender = await UsersManager.RegisterNewUser(update);
            }
            else sender = GetDefaultBotUser(id);

            if (sender is null)
                throw new NullSenderException();

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
                throw new BotManagerExcpetion("cs.NullUpdateHandler", ToString(), Enum.GetName(typeof(TEnum.UpdateType), update.Type));
            
            await suitHandler.HandleUpdateAsync(update, sender);
            
            if (sender is not null && UsersManager is not null && UsersManager.SignedEventHandled is not null)
                await UsersManager.SignedEventHandled.Invoke(sender);
        }

        /// <summary>
        /// Extracts sender's ID from a raw server update or
        /// throws <see cref="BotManagerExcpetion"/> otherwise.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Sender's ID.</returns>
        /// <exception cref="BotManagerExcpetion"></exception>
        public long GetSenderId(Update update) => TryGetSenderId(update)
            ?? throw new BotManagerExcpetion("cs.UserIdExtractError", ToString(), update.Id.ToString());
        /// <summary>
        /// Tries to extract sender's ID from a raw server update.
        /// </summary>
        /// <param name="update">Original server update</param>
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
        /// throws <see cref="BotManagerExcpetion"/> otherwise.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Not-null instance of a sender.</returns>
        /// <exception cref="BotManagerExcpetion"></exception>
        public User GetSender(Update update) => GetSender(update, this);
        /// <summary>
        /// Extracts sender instance of a type <see cref="User"/> from a raw server update or
        /// throws <see cref="BotManagerExcpetion"/> otherwise.
        /// </summary>
        /// <param name="update">Original server update</param>
        /// <returns>Not-null instance of a sender.</returns>
        /// <exception cref="BotManagerExcpetion"></exception>
        public static User GetSender(Update update, object? sender) => TryGetSender(update)
            ?? throw new BotManagerExcpetion("cs.UserExtractError", sender?.ToString() ?? "Unknown object", update.Id.ToString());
        /// <summary>
        /// Tries to extract sender instance of a type <see cref="User"/> from a raw server update.
        /// </summary>
        /// <param name="update">Original server update</param>
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

        public override string? ToString() => DebugName is null
            ? Enum.GetName(typeof(TEnum.ChatType), ChatType)
            : $"{DebugName} ({Owner.DebugName})";
    }
}