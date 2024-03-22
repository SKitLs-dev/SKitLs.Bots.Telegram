using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Model.Users;
using SKitLs.Bots.Telegram.Core.Prototype;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="IUpdateHandlerBase{TUpdate}"/> for handling anonymous message updates.
    /// Uses a system of sub-handlers for different message content types such as text, media, voice, etc. (see <see cref="MessageType"/>).
    /// <para/>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>.
    /// </summary>
    public class AnonymMessageBaseHandler : OwnedObject, IUpdateHandlerBase<AnonymMessageUpdate>
    {
        /// <summary>
        /// The sub-handler used for handling incoming text messages.
        /// </summary>
        public IUpdateHandlerBase<AnonymMessageTextUpdate>? TextMessageUpdateHandler { get; set; }

        /// <summary>
        /// The sub-handler used for handling other incoming message types (PhotoMessage, MediaMessage, etc.).
        /// </summary>
        public IUpdateHandlerBase<AnonymMessageUpdate>? RestMessagesUpdateHandler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymMessageBaseHandler"/> class
        /// with the default implementation of several sub-handlers.
        /// </summary>
        public AnonymMessageBaseHandler()
        {
            TextMessageUpdateHandler = new AnonymMessageTextHandler();
        }

        /// <inheritdoc/>
        public List<IBotAction> GetHeldActions() => TextMessageUpdateHandler?.GetHeldActions() ?? new();

        /// <inheritdoc/>
        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender) => await HandleUpdateAsync(CastUpdate(update, sender));

        /// <inheritdoc/>
        public AnonymMessageUpdate CastUpdate(ICastedUpdate update, IBotUser? sender) => new(update);

        /// <inheritdoc/>
        public async Task HandleUpdateAsync(AnonymMessageUpdate update)
        {
            if (update.Message.Type == MessageType.Text && TextMessageUpdateHandler is not null)
                await TextMessageUpdateHandler.HandleUpdateAsync(new AnonymMessageTextUpdate(update));
            else if (RestMessagesUpdateHandler is not null)
                await RestMessagesUpdateHandler.HandleUpdateAsync(update);
            // TODO: Photo Video Voice etc
        }
    }
}