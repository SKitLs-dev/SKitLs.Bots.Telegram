//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
//using SKitLs.Bots.Telegram.Core.Prototypes;
//using SKitLs.Bots.Telegram.Interactions.Prototype;
//using SKitLs.Bots.Telegram.Management.Managers.Prototype;
//using SKitLs.Bots.Telegram.Stateful.Model;

//namespace SKitLs.Bots.Telegram.Stateful.Management
//{
//    internal class StatefulCommandsManager : ICommandsManager
//    {
//        public StatefulCommandsManager()
//        {
//            CommandsSections = new();
//            DefaultAnswers = new();
//            //DefaultNotEnoughRightsMessage = new OutputMessageDecorText("Недостаточно прав для использования данной команды");
//        }

//        public IOutputMessage? DefaultNotEnoughRightsMessage { get; set; }

//        public List<CommandsStateSection> CommandsSections { get; set; }
//        public void AddCommandsStateSection(CommandsStateSection section) => CommandsSections.Add(section);

//        public List<DefaultStateAnswer> DefaultAnswers { get; set; }
//        public void AddStateDefaultAnswer(DefaultStateAnswer answer)
//        {
//            //List<int> determinedStates = new();
//            //DefaultAnswers.ForEach(dAnswer => dAnswer.StatesId.ForEach(state => determinedStates.Add(state.StateId)));
//            //List<int> applyingStates = answer.StatesId.Select(x => (int)x).ToList();
//            //if (determinedStates.Intersect(applyingStates).ToList().Count == 0)
//            DefaultAnswers.Add(answer);
//        }

//        public async Task HandleUpdateAsync(SignedMessageTextUpdate update)
//        {
//            List<CommandsStateSection> enabledStates =
//                CommandsSections.FindAll(x => x.ShouldBeExecuted(update.Sender.StateId));
//            foreach (CommandsStateSection section in enabledStates)
//                foreach (IBotCommand command in section)
//                {
//                    if (command.ShouldBeExecutedOn(update.Text))
//                    {
//                        await command.Action(command, update);

//                        //if (update.Sender.PermissionLevel >= command.RequiredPermissionLevel)
//                        //else if (command.NotEnoughRightsMessage != null)
//                        //    await update.SendMessageToSender(command.NotEnoughRightsMessage, new());
//                        //else if (DefaultNotEnoughRightsMessage != null)
//                        //    await update.SendMessageToSender(DefaultNotEnoughRightsMessage, new());
//                        return;
//                    }
//                }
//            DefaultStateAnswer? @default = DefaultAnswers.Find(x => x.SuitableStates.Contains(update.Sender.StateId));
//            if (@default != null)
//                await update.SendMessageToSender(@default.Message + $"\nВаш id: {update.Sender.TelegramId}", new());
//        }
//    }
//}
