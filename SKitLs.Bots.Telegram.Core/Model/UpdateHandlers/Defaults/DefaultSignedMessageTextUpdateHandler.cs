using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Core.resources.Settings;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default realization for <see cref="IUpdateHandlerBase"/>&lt;<see cref="SignedMessageTextUpdate"/>&gt;.
    /// Uses a system of <see cref="IActionManager{TUpdate}"/> for handling incoming text updates 
    /// such as: Text input or Commands.
    /// <para>
    /// Inherits: <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/>
    /// </para>
    /// </summary>
    public class DefaultSignedMessageTextUpdateHandler : IUpdateHandlerBase<SignedMessageTextUpdate>
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Actions manager used for handling incoming commands.
        /// <para>
        /// For commands determination check: <see cref="BotSettings.IsCommand"/>.
        /// </para>
        /// </summary>
        public IActionManager<SignedMessageTextUpdate> CommandsManager { get; set; }
        /// <summary>
        /// Actions manager used for handling incoming text.
        /// </summary>
        public IActionManager<SignedMessageTextUpdate> TextInputManager { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="DefaultSignedMessageTextUpdateHandler"/>
        /// with default realization of managers <see cref="DefaultActionManager{TUpdate}"/>.
        /// </summary>
        public DefaultSignedMessageTextUpdateHandler()
        {
            CommandsManager = new DefaultActionManager<SignedMessageTextUpdate>();
            TextInputManager = new DefaultActionManager<SignedMessageTextUpdate>();
        }
        public List<IBotAction> GetActionsContent()
        {
            var res = new List<IBotAction>();
            res.AddRange(CommandsManager.GetActionsContent());
            res.AddRange(TextInputManager.GetActionsContent());
            return res;
        }

        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(CastUpdate(update, sender));
        public SignedMessageTextUpdate CastUpdate(ICastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();
            return new SignedMessageTextUpdate(new SignedMessageUpdate(update, sender));
        }
        public async Task HandleUpdateAsync(SignedMessageTextUpdate update)
        {
            if (update.Owner.Settings.IsCommand(update.Text))
            {
                await CommandsManager.ManageUpdateAsync(update);
            }
            else
            {
                await TextInputManager.ManageUpdateAsync(update);
            }
        }
    }
}