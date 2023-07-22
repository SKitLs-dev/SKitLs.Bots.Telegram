using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    public interface IBotProcess
    {
        public string ProcessDefId { get; }
        public IUserState ProcessState { get; }
    }
    public interface IBotProcess<T> : IBotProcess where T : IProcessArguments
    {
        public IBotRunningProcess GetRunning(long userId, T args);
    }
}