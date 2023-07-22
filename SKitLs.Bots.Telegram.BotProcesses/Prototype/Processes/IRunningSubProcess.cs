namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    public interface IRunningSubProcess<TOwner> where TOwner : IBotRunningProcess
    {
        public int SubProcId { get; }
        public TOwner Owner { get; }
    }
}