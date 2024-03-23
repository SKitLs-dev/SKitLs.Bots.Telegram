using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.Settings;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Users;

namespace SKitLs.Bots.Telegram.Core.UpdateHandlers.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="IUpdateHandlerBase{TUpdate}"/> for handling signed message text updates.
    /// Uses a system of <see cref="IActionManager{TUpdate}"/> for handling incoming text updates such as text input or commands.
    /// <para/>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>.
    /// </summary>
    public class SignedMessageTextHandler : OwnedObject, IUpdateHandlerBase<SignedMessageTextUpdate>
    {
        /// <summary>
        /// The action manager used for handling incoming commands.
        /// Check <see cref="BotSettings.IsCommand"/> for command determination.
        /// </summary>
        public IActionManager<SignedMessageTextUpdate> CommandsManager { get; set; }

        /// <summary>
        /// The action manager used for handling incoming text.
        /// </summary>
        public IActionManager<SignedMessageTextUpdate> TextInputManager { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedMessageTextHandler"/> class
        /// with default implementations of action managers (<see cref="LinearActionManager{TUpdate}"/>).
        /// </summary>
        public SignedMessageTextHandler()
        {
            CommandsManager = new LinearActionManager<SignedMessageTextUpdate>();
            TextInputManager = new LinearActionManager<SignedMessageTextUpdate>();
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
        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(CastUpdate(update, sender));

        /// <inheritdoc/>
        public SignedMessageTextUpdate CastUpdate(ICastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException(this);
            return new SignedMessageTextUpdate(new SignedMessageUpdate(update, sender));
        }

        /// <inheritdoc/>
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