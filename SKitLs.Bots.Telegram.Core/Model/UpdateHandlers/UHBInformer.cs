using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    /// <summary>
    /// Default informer class that should infrom sender about incoming update.
    /// Only use for debugging purposes.
    /// </summary>
    /// <typeparam name="TUpdate">Scecific casted update that this handler should work with.</typeparam>
    [Obsolete("Unsafe. Only use for debugging purposes.", false)]
    public class UHBInformer<TUpdate> : IUpdateHandlerBase<TUpdate> where TUpdate : class, ICastedUpdate
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Name for the handling update to be printed.
        /// </summary>
        public string UpdateName { get; private set; }
        /// <summary>
        /// Determines whether info about update should be printed in logger.
        /// </summary>
        public bool UseLogger { get; private set; }
        /// <summary>
        /// Determines whether info about update should be printed in chat.
        /// </summary>
        public bool InformInChat { get; private set; }

        /// <summary>
        /// Creates a new instance of a <see cref="UHBInformer{TUpdate}"/> with specified data.
        /// </summary>
        /// <param name="updateName">Name of the update to be printed</param>
        /// <param name="log">Should info be printed in logger?</param>
        /// <param name="inform">Should info be printed in chat?</param>
        public UHBInformer(string? updateName = null, bool log = true, bool inform = false)
        {
            UpdateName = updateName ?? typeof(TUpdate).Name;
            UseLogger = log;
            InformInChat = inform;
        }

        // This class does not contain any actions.
        // => Returns new empty list.
        public List<IBotAction> GetActionsContent() => new();

        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(CastUpdate(update, sender));

        public TUpdate CastUpdate(ICastedUpdate update, IBotUser? sender) => (update as TUpdate)
            ?? throw new BotManagerExcpetion("UHBCasting", UpdateName, update.OriginalSource.Id.ToString());

        public async Task HandleUpdateAsync(TUpdate update)
        {
            string mes = $"Handled update (by {nameof(UHBInformer<TUpdate>)}): {UpdateName}";
            if (UseLogger) { Owner.LocalLogger.Log(mes); }
            if (InformInChat) await Owner.DelieveryService.SendMessageToChatAsync(mes, update.ChatId);
        }
    }
}