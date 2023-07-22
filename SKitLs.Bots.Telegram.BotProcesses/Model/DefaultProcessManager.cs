using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model
{
    public class DefaultProcessManager : IProcessManager
    {
        public Dictionary<string, IBotProcess> DefinedProcesses { get; set; } = new();
        public Dictionary<long, IBotRunningProcess> RunningProcesses { get; set; } = new();

        // TODO
        public void Define(IBotProcess process)
        {
            if (DefinedProcesses.ContainsKey(process.ProcessDefId))
                throw new Exception();
            DefinedProcesses.Add(process.ProcessDefId, process);
        }
        public void Define(ICollection<IBotProcess> processes) => processes.ToList().ForEach(x => Define(x));
        public void Define(IProcessIntegratable integratable) => Define(integratable.GetProcesses());
        //TODO
        public IBotProcess GetDefined(string processId) => DefinedProcesses.GetValueOrDefault(processId) ?? throw new Exception();

        public IBotRunningProcess? GetRunning(IStatefulUser sender) => RunningProcesses.GetValueOrDefault(sender.TelegramId);
        // TODO
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, ISignedUpdate update) where T : IProcessArguments
            => update.Sender is IStatefulUser stateful ? Run(process, args, stateful) : throw new Exception();
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, IStatefulUser sender) where T : IProcessArguments
        {
            var running = process.GetRunning(sender.TelegramId, args);
            sender.State = process.ProcessState;
            RunningProcesses.Add(running.OwnerUserId, running);
            return running;
        }
        public void Terminate(IStatefulUser sender)
        {
            sender.ResetState();
            RunningProcesses.Remove(sender.TelegramId);
        }
    }
}