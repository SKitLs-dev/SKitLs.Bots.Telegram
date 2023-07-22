using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype
{
    public interface IProcessIntegratable
    {
        public ICollection<IBotProcess> GetProcesses();
    }
}