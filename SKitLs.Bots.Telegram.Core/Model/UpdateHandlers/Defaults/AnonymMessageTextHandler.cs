using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.resources.Settings;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    // XML-Doc Update
    /// <summary>
    /// Default realization for <see cref="IUpdateHandlerBase"/>&lt;<see cref="AnonymMessageTextUpdate"/>&gt;.
    /// Uses a system of <see cref="IActionManager{TUpdate}"/> for handling incoming text updates 
    /// such as: Text input or Commands.
    /// <para>
    /// Inherits: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>
    /// </para>
    /// </summary>
    public class AnonymMessageTextHandler : IUpdateHandlerBase<AnonymMessageTextUpdate>
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
        /// Actions manager used for handling incoming commands.
        /// <para>
        /// For commands determination check: <see cref="BotSettings.IsCommand"/>.
        /// </para>
        /// </summary>
        public IActionManager<AnonymMessageTextUpdate> CommandsManager { get; set; }
        /// <summary>
        /// Actions manager used for handling incoming text.
        /// </summary>
        public IActionManager<AnonymMessageTextUpdate> TextInputManager { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="SignedMessageTextHandler"/>
        /// with default realization of managers <see cref="DefaultActionManager{TUpdate}"/>.
        /// </summary>
        public AnonymMessageTextHandler()
        {
            CommandsManager = new DefaultActionManager<AnonymMessageTextUpdate>();
            TextInputManager = new DefaultActionManager<AnonymMessageTextUpdate>();
        }

        /// <summary>
        /// Collects all <see cref="IBotAction"/>s declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public List<IBotAction> GetHeldActions()
        {
            var res = new List<IBotAction>();
            res.AddRange(CommandsManager.GetHeldActions());
            res.AddRange(TextInputManager.GetHeldActions());
            return res;
        }

        /// <summary>
        /// Handles <see cref="ICastedUpdate"/> updated, gotten from <see cref="ChatScanner"/>.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(CastUpdate(update, sender));

        /// <summary>
        /// Casts common incoming <see cref="ICastedUpdate"/> to the specified
        /// <see cref="AnonymMessageTextUpdate"/> update type.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        /// <returns>Casted updated oh a type <see cref="AnonymMessageTextUpdate"/>.</returns>
        public AnonymMessageTextUpdate CastUpdate(ICastedUpdate update, IBotUser? sender) => new(new AnonymMessageUpdate(update));

        /// <summary>
        /// Handles custom casted <see cref="AnonymMessageTextUpdate"/> updated.
        /// <para>
        /// Cast and pass update via base <see cref="IUpdateHandlerBase.HandleUpdateAsync(ICastedUpdate, IBotUser?)"/>
        /// </para>
        /// </summary>
        /// <param name="update">Update to handle.</param>
        public async Task HandleUpdateAsync(AnonymMessageTextUpdate update)
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