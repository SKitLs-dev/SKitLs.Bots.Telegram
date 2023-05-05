using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.AdvancedHandlers.Defaults
{
    public class DefaultAnonimMessageTextUpdateHandler : IUpdateHandlerBase<AnonimMessageTextUpdate>
    {
        public BotManager Owner { get; private set; }

        public Func<string, bool> IsCommand { get; set; }
        public IActionManager<IBotAction<AnonimMessageTextUpdate>, AnonimMessageTextUpdate> CommandsManager { get; set; }
        public IActionManager<IBotAction<AnonimMessageTextUpdate>, AnonimMessageTextUpdate> TextInputManager { get; set; }

        public DefaultAnonimMessageTextUpdateHandler(BotManager owner)
        {
            Owner = owner;
            CommandsManager = new DefaultActionManager<AnonimMessageTextUpdate>(owner);
            TextInputManager = new DefaultActionManager<AnonimMessageTextUpdate>(owner);
            IsCommand = (input) => input.StartsWith('/');
        }

        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(BuildUpdate(update, sender));
        public AnonimMessageTextUpdate BuildUpdate(CastedUpdate update, IBotUser? sender) => new(new AnonimMessageUpdate(update));
        
        public async Task HandleUpdateAsync(AnonimMessageTextUpdate update)
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
