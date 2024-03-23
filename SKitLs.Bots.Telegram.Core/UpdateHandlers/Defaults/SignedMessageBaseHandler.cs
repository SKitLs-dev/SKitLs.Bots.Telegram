using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Users;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="IUpdateHandlerBase{TUpdate}"/> for handling signed message updates.
    /// Uses a system of sub-handlers for different message content such as text, media, voice, etc. (see <see cref="MessageType"/>).
    /// <para/>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>.
    /// </summary>
    public class SignedMessageBaseHandler : OwnedObject, IUpdateHandlerBase<SignedMessageUpdate>
    {
        /// <summary>
        /// The sub-handler used for handling incoming text messages.
        /// </summary>
        public IUpdateHandlerBase<SignedMessageTextUpdate>? TextMessageUpdateHandler { get; set; }

        /// <summary>
        /// The sub-handler used for handling other incoming messages (PhotoMessage, MediaMessage, etc.).
        /// </summary>
        public IUpdateHandlerBase<SignedMessageUpdate>? RestMessagesUpdateHandler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedMessageBaseHandler"/> class
        /// with default implementations of several sub-handlers.
        /// </summary>
        public SignedMessageBaseHandler()
        {
            TextMessageUpdateHandler = new SignedMessageTextHandler();
        }

        /// <inheritdoc/>
        public List<IBotAction> GetHeldActions() => TextMessageUpdateHandler?.GetHeldActions() ?? new();

        /// <inheritdoc/>
        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(CastUpdate(update, sender));

        /// <inheritdoc/>
        public SignedMessageUpdate CastUpdate(ICastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException(this);
            return new(update, sender);
        }

        /// <inheritdoc/>
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