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
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        /// <summary>
        /// Specified method that raised during reflective <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/> compilation.
        /// Declare it to extend preset functionality.
        /// Invoked after <see cref="Owner"/> updating, but before recursive update.
        /// </summary>
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

        /// <summary>
        /// Collects all <see cref="IBotAction"/>s declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public List<IBotAction> GetActionsContent() => CallbackManager.GetActionsContent();

        /// <summary>
        /// Handles <see cref="ICastedUpdate"/> updated, gotten from <see cref="ChatScanner"/>.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        public Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => HandleUpdateAsync(CastUpdate(update, sender));

        /// <summary>
        /// Casts common incoming <see cref="ICastedUpdate"/> to the specified
        /// <see cref="SignedCallbackUpdate"/> update type.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        /// <returns>Casted updated oh a type <see cref="SignedCallbackUpdate"/>.</returns>
        public SignedCallbackUpdate CastUpdate(ICastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException(this);
            return new(update, sender);
        }

        /// <summary>
        /// Handles custom casted <see cref="SignedCallbackUpdate"/> updated.
        /// <para>
        /// Cast and pass update via base <see cref="IUpdateHandlerBase.HandleUpdateAsync(ICastedUpdate, IBotUser?)"/>
        /// </para>
        /// </summary>
        /// <param name="update">Update to handle.</param>
        public async Task HandleUpdateAsync(SignedCallbackUpdate update) => await CallbackManager.ManageUpdateAsync(update);
    }
}