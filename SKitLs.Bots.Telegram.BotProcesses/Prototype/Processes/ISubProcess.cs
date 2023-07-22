namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    public interface ISubProcess<TOwner, TArgs> where TOwner : IBotRunningProcess where TArgs : IProcessArguments
    {
        public int SubProcId { get; }
        public bool IsTerminational { get; }

        public IRunningSubProcess<TOwner> GetRunning(TOwner owner, TArgs args);
    }
    public interface ISubProcess<TOwner> where TOwner : IBotRunningProcess
    {
        public int SubProcId { get; }
        public bool IsTerminational { get; }

        public IRunningSubProcess<TOwner> GetRunning(TOwner owner);
    }
}