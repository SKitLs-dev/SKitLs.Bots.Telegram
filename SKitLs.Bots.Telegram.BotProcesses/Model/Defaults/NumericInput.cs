namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults
{
    using global::SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
    using global::SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
    using global::SKitLs.Bots.Telegram.BotProcesses.Prototype;
    using global::SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
    using global::SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
    using global::SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
    using global::SKitLs.Bots.Telegram.Stateful.Prototype;

    namespace SKitLs.Bots.Telegram.DataBases.Model.Processes.Inputs
    {
        public delegate Task SimpleInputProcessComplete<TRes, TUpdate>(TRes args, TUpdate update) where TUpdate : ICastedUpdate, ISignedUpdate;

        public class IntInputProcess : DefaultProcessBase, IBotProcess<ProcNewWrapper<string>>
        {
            public SimpleInputProcessComplete<KeyValuePair<int, bool>, SignedMessageTextUpdate> WhenOver { get; set; }

            public IntInputProcess(string processDefId, string terminationalKey, SimpleInputProcessComplete<KeyValuePair<int, bool>, SignedMessageTextUpdate> whenOver, IUserState controllerState)
                : base(processDefId, terminationalKey, controllerState)
            {
                WhenOver = whenOver;
            }

            public IBotRunningProcess GetRunning(long userId, ProcNewWrapper<string> onLaunchText)
            {
                if (onLaunchText.Value is null)
                    throw new Exception();
                var res = new IntInputProcessRunning(userId, onLaunchText.Value, this);
                return res;
            }
        }

        public class IntInputProcessRunning : IntInputProcess, IBotRunningProcess
        {
            public long OwnerUserId { get; set; }
            public IOutputMessage OnLaunch { get; set; }
            public int Value { get; set; }

            public IntInputProcessRunning(long userId, string onLaunchText, IntInputProcess launcher) : this(userId, new OutputMessageText(onLaunchText), launcher) { }
            public IntInputProcessRunning(long userId, IOutputMessage onLaunch, IntInputProcess launcher)
                : base(launcher.ProcessDefId, launcher.TerminationalKey, launcher.WhenOver, launcher.ProcessState)
            {
                OnLaunch = onLaunch;
                OwnerUserId = userId;
            }

            public async Task LaunchWith<TTrigger>(TTrigger update) where TTrigger : ISignedUpdate
            {
                await update.Owner.DeliveryService.ReplyToSender(OnLaunch, update);
            }
            public async Task HandleInput<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
            {
                if (update is not SignedMessageTextUpdate messageUpdate)
                    throw new Exception();
                if (update.Sender is not IStatefulUser stateful)
                    throw new Exception();

                update.Owner.ResolveService<IProcessManager>().Terminate(stateful);
                await WhenOver.Invoke(new(Value, messageUpdate.Text.ToLower() == TerminationalKey), messageUpdate);
            }
        }
    }
}