//using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Partial;

//namespace SKitLs.Bots.Telegram.BotProcesses.obsolete
//{
//    public static class InputDefaults
//    {
//        public static InputProcessArgs<AcceptationWrapper<T>> GetYNArgs<T>(T value) => new InputProcessArgs<AcceptationWrapper<T>>(new AcceptationWrapper<T>(value));

//        public static PartialInputProcess<AcceptationWrapper<T>> YesNoProcess<T>(string preview, InputProcessComplete<AcceptationWrapper<T>> whenOver, IUserState controllerState, string yesLabel = "Да", string noLabel = "Нет")
//        {
//            var res = new PartialInputProcess<AcceptationWrapper<T>>("systemProcYesNo", noLabel, controllerState, whenOver);
//            var mes = new OutputMessageText(preview)
//            {
//                Menu = new ReplyMenu(yesLabel, noLabel),
//            };
//            res.AddSub(new InputSubProcess<AcceptationWrapper<T>>(typeof(AcceptationWrapper<T>).GetProperty(nameof(AcceptationWrapper<T>.Result))!, mes, ParseBool)
//            {
//                InputPreview = BoolPreview
//            });
//            return res;

//            IBuildableMessage? BoolPreview(SignedMessageTextUpdate update)
//            {
//                var input = update.Text;
//                if (input.ToLower() == yesLabel.ToLower() || input.ToLower() == noLabel.ToLower())
//                    return null;
//                return new OutputMessageText("Непредвиденный ввод.\nВведите, пожалуйста, Да или Нет.")
//                {
//                    Menu = new ReplyMenu(yesLabel, noLabel),
//                };
//            }
//            object ParseBool(SignedMessageTextUpdate update) => update.Text.ToLower() == yesLabel.ToLower();
//        }
//    }
//}