using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    // XML-Doc Update
    /// <summary>
    /// Default informer class that should inform sender about incoming update.
    /// Only use for debugging purposes.
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this handler should work with.</typeparam>
    [Obsolete("Unsafe. Only use for debugging purposes.", false)]
    public class UHBInformer<TUpdate> : IUpdateHandlerBase<TUpdate> where TUpdate : class, ICastedUpdate
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
        /// Name for the handling update to be printed.
        /// </summary>
        public string UpdateName { get; init; }
        /// <summary>
        /// Determines whether info about update should be printed in logger.
        /// </summary>
        public bool UseLogger { get; init; }
        /// <summary>
        /// Determines whether info about update should be printed in chat.
        /// </summary>
        public bool InformInChat { get; init; }

        /// <summary>
        /// Creates a new instance of a <see cref="UHBInformer{TUpdate}"/> with specified data.
        /// </summary>
        /// <param name="updateName">Name of the update to be printed.</param>
        /// <param name="log">Determines either info should be printed in logger.</param>
        /// <param name="inform">Determines either info should be printed in chat.</param>
        public UHBInformer(string? updateName = null, bool log = true, bool inform = false)
        {
            UpdateName = updateName ?? typeof(TUpdate).Name;
            UseLogger = log;
            InformInChat = inform;
        }

        /// <summary>
        /// Returns an 
        /// </summary>
        /// <returns>An empty list, because <see cref="UHBInformer{TUpdate}"/> does not provide methods of executing actions.</returns>
        public List<IBotAction> GetHeldActions() => new();

        /// <summary>
        /// Handles <see cref="ICastedUpdate"/> updated, gotten from <see cref="ChatScanner"/>.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(CastUpdate(update, sender));

        /// <summary>
        /// Casts common incoming <see cref="ICastedUpdate"/> to the specified
        /// <typeparamref name="TUpdate"/> update type.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        /// <returns>Casted updated oh a type <typeparamref name="TUpdate"/>.</returns>
        public TUpdate CastUpdate(ICastedUpdate update, IBotUser? sender) => (update as TUpdate)
            ?? throw new BotManagerException("UHBCasting", UpdateName, update.OriginalSource.Id.ToString());

        /// <summary>
        /// Handles custom casted <typeparamref name="TUpdate"/> updated.
        /// <para>
        /// Cast and pass update via base <see cref="IUpdateHandlerBase.HandleUpdateAsync(ICastedUpdate, IBotUser?)"/>
        /// </para>
        /// </summary>
        /// <param name="update">Update to handle.</param>
        public async Task HandleUpdateAsync(TUpdate update)
        {
            string mes = $"Handled update (by {nameof(UHBInformer<TUpdate>)}): {UpdateName}";
            if (UseLogger) { Owner.LocalLogger.Log(mes); }
            if (InformInChat) await Owner.DeliveryService.SendMessageToChatAsync(update.ChatId, mes);
        }
    }
}