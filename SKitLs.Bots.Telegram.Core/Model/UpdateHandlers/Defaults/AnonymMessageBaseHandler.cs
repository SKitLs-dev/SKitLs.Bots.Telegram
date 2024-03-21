using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Prototype;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    // XML-Doc Update
    /// <summary>
    /// Default realization for <see cref="IUpdateHandlerBase"/>&lt;<see cref="AnonymMessageUpdate"/>&gt;.
    /// Uses a system of sub-<see cref="IUpdateHandlerBase"/> for different message content such as:
    /// text, media, voice etc (see <see cref="MessageType"/>).
    /// <para>
    /// Inherits: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>
    /// </para>
    /// </summary>
    public class AnonymMessageBaseHandler : IUpdateHandlerBase<AnonymMessageUpdate>
    {
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
        /// Sub-handler used for handling incoming Text Messages.
        /// </summary>
        public IUpdateHandlerBase<AnonymMessageTextUpdate>? TextMessageUpdateHandler { get; set; }

        /// <summary>
        /// Sub-handler used for handling other incoming messages (PhotoMessage, MediaMessage, etc).
        /// </summary>
        public IUpdateHandlerBase<AnonymMessageUpdate>? RestMessagesUpdateHandler { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="AnonymMessageBaseHandler"/>
        /// with default realization of several sub-handlers.
        /// </summary>
        public AnonymMessageBaseHandler()
        {
            TextMessageUpdateHandler = new AnonymMessageTextHandler();
        }

        /// <summary>
        /// Collects all <see cref="IBotAction"/>s declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public List<IBotAction> GetHeldActions() => TextMessageUpdateHandler?.GetHeldActions() ?? new();

        /// <summary>
        /// Handles <see cref="ICastedUpdate"/> updated, gotten from <see cref="ChatScanner"/>.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(CastUpdate(update, sender));

        /// <summary>
        /// Casts common incoming <see cref="ICastedUpdate"/> to the specified
        /// <see cref="AnonymMessageUpdate"/> update type.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        /// <returns>Casted updated oh a type <see cref="AnonymMessageUpdate"/>.</returns>
        public AnonymMessageUpdate CastUpdate(ICastedUpdate update, IBotUser? sender) => new(update);

        /// <summary>
        /// Handles custom casted <see cref="AnonymMessageUpdate"/> updated.
        /// <para>
        /// Cast and pass update via base <see cref="IUpdateHandlerBase.HandleUpdateAsync(ICastedUpdate, IBotUser?)"/>
        /// </para>
        /// </summary>
        /// <param name="update">Update to handle.</param>
        public async Task HandleUpdateAsync(AnonymMessageUpdate update)
        {
            if (update.Message.Type == MessageType.Text && TextMessageUpdateHandler is not null)
                await TextMessageUpdateHandler.HandleUpdateAsync(new AnonymMessageTextUpdate(update));
            else if (RestMessagesUpdateHandler is not null)
                await RestMessagesUpdateHandler.HandleUpdateAsync(update);
            // Photo Video Voice etc
        }
    }
}