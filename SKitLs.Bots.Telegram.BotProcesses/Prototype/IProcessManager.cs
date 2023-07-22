using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype
{
    public interface IProcessManager
    {
        public void Define(IBotProcess process);
        public void Define(ICollection<IBotProcess> processes);
        public void Define(IProcessIntegratable integratable);
        public IBotProcess GetDefined(string processId);

        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, ISignedUpdate update) where T : IProcessArguments;
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, IStatefulUser sender) where T : IProcessArguments;
        public IBotRunningProcess? GetRunning(IStatefulUser sender);
        public void Terminate(IStatefulUser senderId);
    }
}