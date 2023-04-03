using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Management.Managers.Model
{
    public class DefaultTextInputManager : IActionManager<IBotTextInput, SignedMessageTextUpdate>
    {
        public List<IBotTextInput> Actions { get; set; }

        public DefaultTextInputManager()
        {
            Actions = new();
        }

        public async Task HandleUpdateAsync(SignedMessageTextUpdate update)
        {
            List<IBotTextInput> inputs = new();
            foreach (IBotTextInput input in Actions)
                if (input.PredicateExecution(update))
                    inputs.Add(input);

            IBotTextInput? executer = inputs.OrderBy(x => x.ExecutionWeight).ToList().FirstOrDefault();
            if (executer != null) await executer.Executer(executer, update);
            else await update.SendMessageTriggerToChatAsync("Неизвестная команда", new());
        }
    }
}