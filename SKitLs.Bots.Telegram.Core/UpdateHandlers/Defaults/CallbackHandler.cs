using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Model.Users;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="IUpdateHandlerBase{TUpdate}"/> for handling signed callback updates.
    /// Uses a system of <see cref="IActionManager{TUpdate}"/> for handling incoming callbacks.
    /// <para/>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>.
    /// </summary>
    public class CallbackHandler : OwnedObject, IUpdateHandlerBase<SignedCallbackUpdate>
    {
        /// <summary>
        /// The actions manager used for handling incoming callbacks.
        /// </summary>
        public IActionManager<SignedCallbackUpdate> CallbackManager { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackHandler"/> class
        /// with the default implementation of the manager.
        /// </summary>
        public CallbackHandler()
        {
            CallbackManager = new LinearActionManager<SignedCallbackUpdate>();
        }

        /// <inheritdoc/>
        public List<IBotAction> GetHeldActions() => CallbackManager.GetHeldActions();

        /// <inheritdoc/>
        public Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender) => HandleUpdateAsync(CastUpdate(update, sender));

        /// <inheritdoc/>
        public SignedCallbackUpdate CastUpdate(ICastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException(this);
            return new(update, sender);
        }

        /// <inheritdoc/>
        public async Task HandleUpdateAsync(SignedCallbackUpdate update) => await CallbackManager.ManageUpdateAsync(update);
    }
}