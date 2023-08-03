//using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Partial;
//using SKitLs.Bots.Telegram.BotProcesses.Prototype;
//using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
//using SKitLs.Bots.Telegram.Stateful.Prototype;

//namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes
//{
//    [Obsolete("Use TextInputsProcessBase instead", true)]
//    public abstract class InputProcess<TArgs> : TextInputsProcessBase, IBotProcess<TArgs> where TArgs : IProcessArgument
//    {
//        public virtual InputProcessCompleted<TArgs> WhenOver { get; protected set; }

//        public InputProcess(string processDefId, string terminationalKey, IUserState controllerState, InputProcessCompletes<TArgs> whenOver) : base(processDefId, terminationalKey, controllerState)
//        {
//            WhenOver = whenOver;
//        }

//        public virtual async Task Terminate(InputProcessResult<TArgs> result, SignedMessageTextUpdate update)
//        {
//            if (update.Sender is not IStatefulUser stateful)
//                throw new Exception();
//            update.Owner.ResolveService<IProcessManager>().Terminate(stateful);
//            await WhenOver.Invoke(result, update);
//        }
//        public abstract IBotRunningProcess GetRunning(long userId, TArgs args);
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <typeparam name="TRes"></typeparam>
//    /// Очередной класс-прослойка для приведения TUpdate к SignedMessageTextUpdate. В целом, оно уже такое и проверка избыточна,
//    /// но бережённого бог бережёт
//    public abstract class InputRunning<TRes> : InputProcess<TRes>, IBotRunningProcess
//    {
//        public long OwnerUserId { get; private set; }
//        public TRes BuildingInstance { get; protected set; }

//        public InputRunning(long userId, TRes instance, InputProcess<TRes> launcher)
//            : base(launcher.ProcessDefId, launcher.TerminationalKey, launcher.ProcessState, launcher.WhenOver)
//        {
//            OwnerUserId = userId;
//            BuildingInstance = instance;
//        }

//        public abstract Task LaunchWith<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate;

//        public async Task HandleInput<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
//        {
//            if (update is not SignedMessageTextUpdate messageUpdate)
//                throw new Exception();
//            await HandleInput(messageUpdate);
//        }
//        public abstract Task HandleInput(SignedMessageTextUpdate messageTextUpdate);

//        public override IBotRunningProcess GetRunning(long userId, InputProcessArgs<TRes> args) => this;
//    }
//}