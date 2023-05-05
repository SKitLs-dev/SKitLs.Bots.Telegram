using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.AdvancedHandlers.Defaults
{
    public class DefaultSignedMessageTextUpdateHandler : IUpdateHandlerBase<SignedMessageTextUpdate>
    {
        public BotManager Owner { get; private set; }
        public Func<string, bool> IsCommand { get; set; }
        public IActionManager<IBotAction<SignedMessageTextUpdate>, SignedMessageTextUpdate> CommandsManager { get; set; }
        public IActionManager<IBotAction<SignedMessageTextUpdate>, SignedMessageTextUpdate> TextInputManager { get; set; }

        public DefaultSignedMessageTextUpdateHandler(BotManager owner)
        {
            Owner = owner;
            CommandsManager = new DefaultActionManager<SignedMessageTextUpdate>(owner);
            TextInputManager = new DefaultActionManager<SignedMessageTextUpdate>(owner);
            IsCommand = (input) => input.StartsWith('/');
        }

        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(BuildUpdate(update, sender));
        public SignedMessageTextUpdate BuildUpdate(CastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();

            return new SignedMessageTextUpdate(new SignedMessageUpdate(update, sender));
        }
        public async Task HandleUpdateAsync(SignedMessageTextUpdate update)
        {
            if (IsCommand(update.Text))
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
