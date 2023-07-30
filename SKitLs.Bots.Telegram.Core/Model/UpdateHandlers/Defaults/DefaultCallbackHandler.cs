using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default realization for <see cref="IUpdateHandlerBase"/>&lt;<see cref="SignedCallbackUpdate"/>&gt;.
    /// Uses a system of <see cref="IActionManager{TUpdate}"/> for handling incoming Callbacks.
    /// <para>
    /// Inherits: <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/>
    /// </para>
    /// </summary>
    public class DefaultCallbackHandler : IUpdateHandlerBase<SignedCallbackUpdate>
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Actions manager used for handling incoming callbacks.
        /// </summary>
        public IActionManager<SignedCallbackUpdate> CallbackManager { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="DefaultCallbackHandler"/>
        /// with default realization of manager.
        /// </summary>
        public DefaultCallbackHandler()
        {
            CallbackManager = new DefaultActionManager<SignedCallbackUpdate>();
        }
        public List<IBotAction> GetActionsContent() => CallbackManager.GetActionsContent();

        public Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => HandleUpdateAsync(CastUpdate(update, sender));
        public SignedCallbackUpdate CastUpdate(ICastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();
            return new(update, sender);
        }
        public async Task HandleUpdateAsync(SignedCallbackUpdate update) => await CallbackManager.ManageUpdateAsync(update);
    }
}