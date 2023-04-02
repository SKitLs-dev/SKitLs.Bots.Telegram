using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Interactions.Prototype;
using SKitLs.Bots.Telegram.Management.Managers.Prototype;
using SKitLs.Bots.Telegram.Stateful.Model;

namespace SKitLs.Bots.Telegram.Stateful.Management
{
    internal class StatefulTextInputManager : ITextInputManager
    {
        public List<InputStateSection> Inputs { get; set; }

        public StatefulTextInputManager()
        {
            Inputs = new();
        }

        public void AddInputStateSection(InputStateSection section) => Inputs.Add(section);

        public async Task HandleUpdateAsync(SignedMessageTextUpdate update)
        {
            List<IBotTextInput> inputs = new();
            foreach (InputStateSection stateSection in Inputs)
                foreach (IBotTextInput input in stateSection)
                    if (input.ShouldBeExecutedOn(update))
                        inputs.Add(input);
            IBotTextInput? executer = inputs.OrderBy(x => x.ExecutionWeight).ToList().FirstOrDefault();
            
            if (executer != null) await executer.Executer(executer, update);
            else await update.SendMessageTriggerToChatAsync("Неизвестная команда", new());
        }
    }
}