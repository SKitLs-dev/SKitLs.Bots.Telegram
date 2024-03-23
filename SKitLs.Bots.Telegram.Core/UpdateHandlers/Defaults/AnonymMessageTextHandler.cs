using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.Settings;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Anonym;
using SKitLs.Bots.Telegram.Core.Users;

namespace SKitLs.Bots.Telegram.Core.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="IUpdateHandlerBase{TUpdate}"/> for handling anonymous message text updates.
    /// Uses a system of <see cref="IActionManager{TUpdate}"/> for handling incoming text updates such as text input or commands.
    /// <para/>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>.
    /// </summary>
    public class AnonymMessageTextHandler : OwnedObject, IUpdateHandlerBase<AnonymMessageTextUpdate>
    {
        /// <summary>
        /// The actions manager used for handling incoming commands.
        /// For command determination, check: <see cref="BotSettings.IsCommand"/>.
        /// </summary>
        public IActionManager<AnonymMessageTextUpdate> CommandsManager { get; set; }

        /// <summary>
        /// The actions manager used for handling incoming text.
        /// </summary>
        public IActionManager<AnonymMessageTextUpdate> TextInputManager { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymMessageTextHandler"/> class
        /// with the default implementation of managers (<see cref="LinearActionManager{TUpdate}"/>).
        /// </summary>
        public AnonymMessageTextHandler()
        {
            CommandsManager = new LinearActionManager<AnonymMessageTextUpdate>();
            TextInputManager = new LinearActionManager<AnonymMessageTextUpdate>();
        }

        /// <inheritdoc/>
        public List<IBotAction> GetHeldActions()
        {
            var res = new List<IBotAction>();
            res.AddRange(CommandsManager.GetHeldActions());
            res.AddRange(TextInputManager.GetHeldActions());
            return res;
        }

        /// <inheritdoc/>
        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender) => await HandleUpdateAsync(CastUpdate(update, sender));

        /// <inheritdoc/>
        public AnonymMessageTextUpdate CastUpdate(ICastedUpdate update, IBotUser? sender) => new(new AnonymMessageUpdate(update));

        /// <inheritdoc/>
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