using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    public interface IBotRunningProcess
    {
        public long OwnerUserId { get; }
        public Task LaunchWith<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate;
        public Task HandleInput<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate;
    }
}