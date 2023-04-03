using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Interactions.Prototype;
using SKitLs.Bots.Telegram.Management.Managers;
using SKitLs.Bots.Telegram.Management.Managers.Model;

namespace SKitLs.Bots.Telegram.Management.AdvancedHandlers.Model
{
    public class DefaultAnonimMessageTextUpdateHandler : IUpdateHandlerBase<AnonimMessageTextUpdate>
    {
        public Func<string, bool> IsCommand { get; set; }
        public IActionManager<IBotCommand, SignedMessageTextUpdate> CommandsManager { get; set; }
        public IActionManager<IBotTextInput, SignedMessageTextUpdate> TextInputManager { get; set; }

        public DefaultAnonimMessageTextUpdateHandler()
        {
            CommandsManager = new DefaultCommandsManager();
            TextInputManager = new DefaultTextInputManager();
            IsCommand = (input) => input.StartsWith('/');
        }


        public AnonimMessageTextUpdate BuildUpdate(CastedUpdate update, IBotUser? sender)
        {
            throw new NotImplementedException();
        }

        public Task HandleUpdateAsync(AnonimMessageTextUpdate update)
        {
            throw new NotImplementedException();
        }
        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();
            
            await HandleUpdateAsync(new SignedMessageTextUpdate(new SignedMessageUpdate(update, sender)));
        }
        public async Task HandleUpdateAsync(SignedMessageTextUpdate update)
        {
            if (IsCommand(update.Text))
            {
                await CommandsManager.HandleUpdateAsync(update);
            }
            else
            {
                await TextInputManager.HandleUpdateAsync(update);
            }
        }
    }
}
