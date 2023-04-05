using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.AdvancedHandlers.Defaults
{
    public class DefaultAnonimMessageTextUpdateHandler : IUpdateHandlerBase<AnonimMessageTextUpdate>
    {
        public BotManager Owner { get; private set; }

        public Func<string, bool> IsCommand { get; set; }
        public IActionManager<IBotCommand, SignedMessageTextUpdate> CommandsManager { get; set; }
        public IActionManager<IBotTextInput, SignedMessageTextUpdate> TextInputManager { get; set; }

        public DefaultAnonimMessageTextUpdateHandler(BotManager owner)
        {
            Owner = owner;
            CommandsManager = new DefaultCommandsManager();
            TextInputManager = new DefaultTextInputManager();
            IsCommand = (input) => input.StartsWith('/');
        }


        public AnonimMessageTextUpdate BuildUpdate(CastedUpdate update, IBotUser? sender)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();

            await HandleUpdateAsync(new SignedMessageTextUpdate(new SignedMessageUpdate(update, sender)));
        }
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
