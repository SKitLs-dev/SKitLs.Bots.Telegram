using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default realization for <see cref="IUpdateHandlerBase"/>&lt;<see cref="AnonimMessageTextUpdate"/>&gt;.
    /// Uses a system of <see cref="IActionManager{TUpdate}"/> for handling incoming text updates 
    /// such as: Text input or Commands.
    /// <para>
    /// Inherits: <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/>
    /// </para>
    /// </summary>
    public class DefaultAnonimMessageTextUpdateHandler : IUpdateHandlerBase<AnonimMessageTextUpdate>
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
        public IActionManager<AnonimMessageTextUpdate> CommandsManager { get; set; }
        /// <summary>
        /// Actions manager used for handling incoming text.
        /// </summary>
        public IActionManager<AnonimMessageTextUpdate> TextInputManager { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="DefaultSignedMessageTextUpdateHandler"/>
        /// with default realization of managers <see cref="DefaultActionManager{TUpdate}"/>.
        /// </summary>
        public DefaultAnonimMessageTextUpdateHandler()
        {
            CommandsManager = new DefaultActionManager<AnonimMessageTextUpdate>();
            TextInputManager = new DefaultActionManager<AnonimMessageTextUpdate>();
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
        public AnonimMessageTextUpdate CastUpdate(ICastedUpdate update, IBotUser? sender) => new(new AnonimMessageUpdate(update));

        public async Task HandleUpdateAsync(AnonimMessageTextUpdate update)
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