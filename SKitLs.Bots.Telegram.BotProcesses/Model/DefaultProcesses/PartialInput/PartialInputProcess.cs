using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses.PartialInput
{
    public delegate Task InputProcessComplete<TRes>(InputProcessResult<TRes> args, SignedMessageTextUpdate update);

    public static class InputDefaults
    {
        public static InputProcessArgs<AcceptationWrapper<T>> GetYNArgs<T>(T value) => new InputProcessArgs<AcceptationWrapper<T>>(new AcceptationWrapper<T>(value));

        public static PartialInputProcess<AcceptationWrapper<T>> YesNoProcess<T>(string preview, InputProcessComplete<AcceptationWrapper<T>> whenOver, IUserState controllerState, string yesLabel = "Да", string noLabel = "Нет")
        {
            var res = new PartialInputProcess<AcceptationWrapper<T>>("systemProcYesNo", noLabel, controllerState, whenOver);
            var mes = new OutputMessageText(preview)
            {
                Menu = new ReplyMenu(yesLabel, noLabel),
            };
            res.AddSub(new InputSubProcess<AcceptationWrapper<T>>(typeof(AcceptationWrapper<T>).GetProperty(nameof(AcceptationWrapper<T>.Result))!, mes, ParseBool)
            {
                InputPreview = BoolPreview
            });
            return res;

            IBuildableMessage? BoolPreview(SignedMessageTextUpdate update)
            {
                var input = update.Text;
                if (input.ToLower() == yesLabel.ToLower() || input.ToLower() == noLabel.ToLower())
                    return null;
                return new OutputMessageText("Непредвиденный ввод.\nВведите, пожалуйста, Да или Нет.")
                {
                    Menu = new ReplyMenu(yesLabel, noLabel),
                };
            }
            object ParseBool(SignedMessageTextUpdate update) => update.Text.ToLower() == yesLabel.ToLower();
        }
    }
    public class PartialInputProcess<TRes> : InputProcess<TRes>
    {
        private readonly List<InputSubProcess<TRes>> subProcesses = new();
        public IReadOnlyList<InputSubProcess<TRes>> SubProcesses => subProcesses.OrderBy(x => x.SubProcId).ToList();

        public PartialInputProcess(string processDefId, string terminationalKey, IUserState controllerState, InputProcessComplete<TRes> whenOver)
            : base(processDefId, terminationalKey, controllerState, whenOver) { }

        public void AddSub(InputSubProcess<TRes> sub)
        {
            sub.SubProcId = subProcesses.Count;
            subProcesses.Add(sub);
        }
        public void AddSubRange(IEnumerable<InputSubProcess<TRes>> subs)
        {
            foreach (var sub in subs)
                AddSub(sub);
        }

        public override IBotRunningProcess GetRunning(long userId, InputProcessArgs<TRes> args) => new PartialInputRunning<TRes>(userId, args.Value, this);
    }
    public class AcceptationWrapper<T>
    {
        public T Value { get; set; }
        public bool Result { get; set; }
        public AcceptationWrapper(T value) => Value = value;
    }

    public class PartialInputRunning<TResult> : InputRunning<TResult>, IBotRunningProcess
    {
        public PartialInputProcess<TResult> Launcher { get; private set; }
        private int CurrentId { get; set; }

        public IReadOnlyList<InputSubProcess<TResult>> SubProcesses => Launcher.SubProcesses;
        private InputSubProcessRunning<TResult> Current => (InputSubProcessRunning<TResult>)SubProcesses[CurrentId].GetRunning(this);
        public override InputProcessComplete<TResult> WhenOver => Launcher.WhenOver;

        public bool ShouldExecute(InputSubProcess<TResult> asker, string input)
            => input.ToLower() == TerminationalKey.ToLower()
            ? asker.IsTerminational
            : asker.SubProcId == CurrentId;

        public PartialInputRunning(long userId, TResult instance, PartialInputProcess<TResult> launcher)
            : base(userId, instance, launcher)
        {
            Launcher = launcher;
        }

        public override async Task LaunchWith<TUpdate>(TUpdate update)
        {
            var mes = Current.OnLaunch(update);

            if (update is SignedCallbackUpdate callback)
            {
                mes = new EditWrapper(mes, callback.TriggerMessageId);
            }
            await update.Owner.DeliveryService.ReplyToSender(mes, update);
        }
        public override async Task HandleInput(SignedMessageTextUpdate update)
        {
            if (update.Text.ToLower() == TerminationalKey.ToLower())
            {
                var result = InputProcessResult<TResult>.Canceled();
                await Terminate(result, update);
            }
            else
            {
                await Current.HandleInput(update);
            }
        }
        internal async Task Valid(SignedMessageTextUpdate update)
        {
            CurrentId++;
            if (CurrentId < SubProcesses.Count)
            {
                var mes = Current.OnLaunch.Invoke(update);
                await update.Owner.DeliveryService.ReplyToSender(mes, update);
            }
            else
            {
                var result = InputProcessResult<TResult>.Accepted(BuildingInstance);
                await Terminate(result, update);
            }
        }
    }
}