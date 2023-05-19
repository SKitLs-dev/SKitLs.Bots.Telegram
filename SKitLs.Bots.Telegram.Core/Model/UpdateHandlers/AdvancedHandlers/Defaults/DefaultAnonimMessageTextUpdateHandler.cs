using SKitLs.Bots.Telegram.Core.Exceptions;
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
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public Func<string, bool> IsCommand { get; set; }
        public IActionManager<AnonimMessageTextUpdate> CommandsManager { get; set; }
        public IActionManager<AnonimMessageTextUpdate> TextInputManager { get; set; }

        public DefaultAnonimMessageTextUpdateHandler()
        {
            CommandsManager = new DefaultActionManager<AnonimMessageTextUpdate>();
            TextInputManager = new DefaultActionManager<AnonimMessageTextUpdate>();
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
