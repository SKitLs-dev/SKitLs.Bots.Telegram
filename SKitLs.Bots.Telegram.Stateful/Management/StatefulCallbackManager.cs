using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Interactions.Prototype;
using SKitLs.Bots.Telegram.Management.Managers.Prototype;
using SKitLs.Bots.Telegram.Stateful.Model;

namespace SKitLs.Bots.Telegram.Stateful.Management
{
    internal class StatefulCallbackManager : ICallbackManager
    {
        public StatefulCallbackManager()
        {
            CallbacksSections = new();
            DefaultAnswers = new();
            //DefaultNotEnoughRightsMessage = new OutputMessageDecorText("Недостаточно прав для использования данной команды");
        }

        public IOutputMessage? DefaultNotEnoughRightsMessage { get; set; }

        public List<CallbacksStateSection> CallbacksSections { get; set; }
        public void AddCallbacksStateSection(CallbacksStateSection section) => CallbacksSections.Add(section);

        public List<DefaultStateAnswer> DefaultAnswers { get; set; }
        public void AddStateDefaultAnswer(DefaultStateAnswer answer)
        {
            //List<int> determinedStates = new();
            //DefaultAnswers.ForEach(dAnswer => dAnswer.StatesId.ForEach(state => determinedStates.Add(state.StateId)));
            //List<int> applyingStates = answer.StatesId.Select(x => (int)x).ToList();
            //if (determinedStates.Intersect(applyingStates).ToList().Count == 0)
            DefaultAnswers.Add(answer);
        }

        public async Task HandleUpdateAsync(SignedCallbackUpdate update)
        {
            List<CallbacksStateSection> enabledStates =
                CallbacksSections.FindAll(x => x.ShouldBeExecuted(update.Sender.StateId));
            foreach (CallbacksStateSection section in enabledStates)
                foreach (IBotCallback callback in section)
                {
                    if (callback.ShouldBeExecutedOn(update.Data))
                    {
                        await callback.Action(callback, update);

                        //if (update.Sender.PermissionLevel >= callback.RequiredPermissionLevel)
                        //else if (callback.NotEnoughRightsMessage != null)
                        //    await update.SendMessageToSender(callback.NotEnoughRightsMessage, new());
                        //else if (DefaultNotEnoughRightsMessage != null)
                        //    await update.SendMessageToSender(DefaultNotEnoughRightsMessage, new());
                        return;
                    }
                }
            DefaultStateAnswer? @default = DefaultAnswers.Find(x => x.SuitableStates.Contains(update.Sender.StateId));
            if (@default != null)
            {
                //if (@default.Message is IOutputEdit edit)
                //{
                //    edit.EditMessageId = update.TriggerMessageId;
                //    await update.SendMessageToSender(@default.Message, new CancellationTokenSource());
                //}
                //else
                //    await update.SendMessageToSender(@default.Message, new CancellationTokenSource());
            }
        }
    }
}
