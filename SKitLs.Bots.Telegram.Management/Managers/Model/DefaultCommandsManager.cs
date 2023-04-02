using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Management.Managers.Model
{
    internal class DefaultCommandsManager : IActionManager<IBotCommand, SignedMessageTextUpdate>
    {
        public List<IBotCommand> Actions { get; set; }

        public DefaultCommandsManager()
        {
            Actions = new();
        }

        public async Task HandleUpdateAsync(SignedMessageTextUpdate update)
        {
            foreach (IBotCommand command in Actions)
                if (command.ShouldBeExecutedOn(update.Text))
                    await command.Action(command, update);
        }
    }
}