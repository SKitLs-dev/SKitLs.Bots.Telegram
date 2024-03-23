using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Users;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TEnum = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// Represents a <see cref="ChatScanner"/> used for handling updates in different chat types (<see cref="TEnum.ChatType"/>)
    /// such as: Private, Group, Channel etc.
    /// This class determines how the bot should react to different triggers in defined chat types.
    /// It is released in the context of <see cref="BotManager"/>.
    /// <para/>
    /// This class represents the <b>second level</b> of the bot's architecture.
    /// <list type="number">
    ///     <item>
    ///         <term><see cref="BotManager"/></term>
    ///         <description>Main bot's manager. Receives updates, handles, and delegates them to sub-managers.</description>
    ///     </item>
    ///     <item>
    ///         <b><see cref="ChatScanner"/></b>
    ///     </item>
    ///     <item>
    ///         <term><see cref="IUpdateHandlerBase"/></term>
    ///         <description>Provides common mechanisms for updates handling.</description>
    ///     </item>
    /// </list>
    /// Supports: <see cref="IDebugNamed"/>, <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>
    /// </summary>
    public sealed class ChatScanner : OwnedObject, IDebugNamed, IBotActionsHolder
    {
        /// <summary>
        /// Event that occurs when an update is received by the <see cref="ChatScanner"/>.
        /// </summary>
        public event BotInteraction<ICastedUpdate>? UpdateReceived;

        /// <summary>
        /// Event that occurs when an update is handled by the <see cref="ChatScanner"/>.
        /// </summary>
        public event BotInteraction<ICastedUpdate>? UpdateHandled;

        #region Properties

        private string? _debugName;
        /// <inheritdoc/>
        public string DebugName
        {
            get => _debugName ?? $"{Enum.GetName(typeof(TEnum.ChatType), ChatType) ?? "Unknown"} {nameof(ChatScanner)}";
            set => _debugName = value;
        }

        /// <summary>
        /// Gets or sets the type of chat that the current scanner is bound to.
        /// </summary>
        public TEnum.ChatType ChatType { get; internal set; }
        #endregion

        #region Users Handlers

        /// <summary>
        /// Represents a custom service used to manage authorized users. Can be implemented with vanilla roles and
        /// permissions schema. Used to connect internal mechanisms with external databases.
        /// Does not have a default implementation.
        /// </summary>
        public IUsersManager? UsersManager { get; internal set; }

        /// <summary>
        /// Represents the default function used to handle updates and pass <see cref="IBotUser"/> instance to the next
        /// levels. Gets Telegram ID as an argument and returns an instance of <see cref="DefaultBotUser"/> by default.
        /// <para>
        /// Used only in case <c><see cref="UsersManager"/> = null</c> or manager's internal problems.
        /// </para>
        /// </summary>
        public Func<long, IBotUser> GetDefaultBotUser { get; internal set; }
        #endregion

        // TODO: Make generic via Enum.GetValues() of UpdateType enum.
        #region Update Handlers

        /// <summary>
        /// Gets or sets the update handler for signed callback updates.
        /// </summary>
        public IUpdateHandlerBase<SignedCallbackUpdate>? CallbackHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for signed message updates.
        /// </summary>
        public IUpdateHandlerBase<SignedMessageUpdate>? MessageHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for edited signed message updates.
        /// </summary>
        public IUpdateHandlerBase<SignedMessageUpdate>? EditedMessageHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for anonymous channel post updates.
        /// </summary>
        public IUpdateHandlerBase<AnonymMessageUpdate>? ChannelPostHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for edited anonymous channel post updates.
        /// </summary>
        public IUpdateHandlerBase<AnonymMessageUpdate>? EditedChannelPostHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted chat join request updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? ChatJoinRequestHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted chat member updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? ChatMemberHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted chosen inline result updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? ChosenInlineResultHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted inline query updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? InlineQueryHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted my chat member updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? MyChatMemberHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted poll updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? PollHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted poll answer updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? PollAnswerHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted pre-checkout query updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? PreCheckoutQueryHandler { get; set; }

        /// <summary>
        /// Gets or sets the update handler for casted shipping query updates.
        /// </summary>
        public IUpdateHandlerBase<CastedUpdate>? ShippingQueryHandler { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatScanner"/> class.
        /// </summary>
        internal ChatScanner()
        {
            GetDefaultBotUser = (id) => new DefaultBotUser(id, false, "", "Default Bot User");
        }

        /// <inheritdoc/>
        public List<IBotAction> GetHeldActions()
        {
            var res = new List<IBotAction>();
            GetDeclaredHandlers().ForEach(x => res.AddRange(x.GetHeldActions()));
            return res;
        }

        /// <summary>
        /// Handles incoming updates asynchronously. Verifies or builds the sender, casts the update
        /// and delegates it to one of the specific sub-handlers.
        /// <para/>
        /// This method can be useful for obtaining declared handlers.
        /// </summary>
        /// <param name="update">The incoming update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="NullSenderException">Thrown when the sender is null.</exception>
        /// <exception cref="SKTgException">Thrown when there is an error in the bot manager.</exception>
        public async Task HandleUpdateAsync(ICastedUpdate update)
        {
            if (UpdateReceived is not null)
                await UpdateReceived.Invoke(update);

            IBotUser? sender = null;
            var id = TelegramHelper.GetSenderId(update.OriginalSource, this);
            if (UsersManager is not null)
            {
                if (await UsersManager.CheckIfRegisteredAsync(id))
                    sender = await UsersManager.GetUserByIdAsync(id);
                else
                    sender = await UsersManager.RegisterNewUserAsync(update);
            }
            else
                sender = GetDefaultBotUser(id);

            if (sender is null)
                throw new NullSenderException(this);

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

            if (suitHandler is null)
                throw new SKTgException("cs.NullUpdateHandler", SKTEOriginType.Inexternal, ToString(), Enum.GetName(typeof(TEnum.UpdateType), update.Type));
            
            await suitHandler.HandleUpdateAsync(update, sender);

            if (UpdateHandled is not null)
                await UpdateHandled(update);
            
            if (sender is not null && UsersManager is not null && UsersManager.SignedUpdateHandled is not null)
                await UsersManager.SignedUpdateHandled.Invoke(sender, update);
        }

        /// <summary>
        /// Collects all defined non-null handlers into a collection.
        /// </summary>
        /// <returns>A collection of defined handlers.</returns>
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

        /// <inheritdoc/>
        public override string? ToString() => $"{DebugName} ({Owner})";
    }
}