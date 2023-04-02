using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Management.AdvancedHandlers.Prototype;
using SKitLs.Bots.Telegram.Management.Managers.Model;
using SKitLs.Bots.Telegram.Management.Managers.Prototype;

namespace SKitLs.Bots.Telegram.Management.AdvancedHandlers.Model
{
    internal class DefaultTextMessageUpdateHandler : ITextMessageUpdateHandler
    {
        public Func<string, bool> IsCommand { get; set; }
        public ICommandsManager CommandsManager { get; set; }
        public ITextInputManager TextInputManager { get; set; }

        public DefaultTextMessageUpdateHandler()
        {
            CommandsManager = new DefaultCommandsManager();
            TextInputManager = new DefaultTextInputManager();
            IsCommand = (input) => input.StartsWith('/');
        }

        public async Task HandleUpdateAsync(CastedChatUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();
            
            await HandleUpdateAsync(new SignedMessageTextUpdate(update.Owner, new SignedMessageUpdate(update.Owner, update, sender)));
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
