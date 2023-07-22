using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.BotProcesses.Model.Args;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses
{
    public class YesNoProcess<TUpdate> : IBotProcess where TUpdate : ICastedUpdate, ISignedUpdate
    {
        public string ProcessDefId { get; private set; }
        public IUserState ProcessState { get; private set; }
        public BotArgedInteraction<ConfrimArg<TUpdate>, TUpdate> OnYes { get; private set; }
        public BotArgedInteraction<ConfrimArg<TUpdate>, TUpdate> OnNo { get; private set; }
        public BotArgedInteraction<ConfrimArg<TUpdate>, TUpdate>? OnAny { get; private set; }

        public YesNoProcess(string processDefId, IUserState processState, BotArgedInteraction<ConfrimArg<TUpdate>, TUpdate> yes, BotArgedInteraction<ConfrimArg<TUpdate>, TUpdate> no, BotArgedInteraction<ConfrimArg<TUpdate>, TUpdate>? any)
        {
            ProcessDefId = processDefId;
            ProcessState = processState;
            OnYes = yes;
            OnNo = no;
            OnAny = any;
        }

        public async Task Execute(ConfrimArg<TUpdate> args, TUpdate update)
        {
            await (args.Confirm ? OnYes : OnNo).Invoke(args, update);

            if (OnAny is not null)
                await OnAny.Invoke(args, update);
        }
    }

    public class YesNoRunning<TUpdate> : IBotProcess, IBotRunningProcess where TUpdate : ISignedUpdate
    {
        public YesNoProcess<TUpdate> Launcher { get; private set; }
        public string ProcessDefId => Launcher.ProcessDefId;
        public IUserState ProcessState => Launcher.ProcessState;
        public long OwnerUserId { get; private set; }



        public Task HandleInput<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
        {
            throw new NotImplementedException();
        }

        public Task LaunchWith<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
        {
            throw new NotImplementedException();
        }
    }
}