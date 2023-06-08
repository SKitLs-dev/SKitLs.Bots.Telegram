using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default realization for <see cref="IUpdateHandlerBase"/>&lt;<see cref="SignedMessageUpdate"/>&gt;.
    /// Uses a system of sub-<see cref="IUpdateHandlerBase"/> for different message content such as:
    /// text, media, voice etc (see <see cref="MessageType"/>).
    /// <para>
    /// Inherits: <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/>
    /// </para>
    /// </summary>
    public class DefaultSignedMessageUpdateHandler : IUpdateHandlerBase<SignedMessageUpdate>
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Subhandler used for handling incoming Text Messages.
        /// </summary>
        public IUpdateHandlerBase<SignedMessageTextUpdate>? TextMessageUpdateHandler { get; set; }
        
        // PhotoMessage
        // MediaMessage
        // etc
        /// <summary>
        /// Subhandler used for handling other incoming messages.
        /// </summary>
        public IUpdateHandlerBase<SignedMessageUpdate>? RestMessagesUpdateHandler { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="DefaultSignedMessageUpdateHandler"/>
        /// with default realization of several subhandlers.
        /// </summary>
        public DefaultSignedMessageUpdateHandler()
        {
            TextMessageUpdateHandler = new DefaultSignedMessageTextUpdateHandler();
        }
        public List<IBotAction> GetActionsContent() => TextMessageUpdateHandler?.GetActionsContent() ?? new();

        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(CastUpdate(update, sender));

        public SignedMessageUpdate CastUpdate(ICastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();
            return new(update, sender);
        }
        public async Task HandleUpdateAsync(SignedMessageUpdate update)
        {
            if (update.Message.Type == MessageType.Text && TextMessageUpdateHandler is not null)
                await TextMessageUpdateHandler.HandleUpdateAsync(new SignedMessageTextUpdate(update));
            else if (RestMessagesUpdateHandler is not null)
                await RestMessagesUpdateHandler.HandleUpdateAsync(update);
            // Photo Video Voice etc
        }
    }
}