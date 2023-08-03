using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    /// <summary>
    /// <see cref="IBotRunningProcess"/> represents a running instance of a bot process.
    /// </summary>
    public interface IBotRunningProcess : IBotProcess
    {
        /// <summary>
        /// Represents the unique identifier of the user who owns and initiated the running bot process.
        /// It allows to identify and associate the bot process with the specific user who triggered its execution.
        /// </summary>
        public long OwnerUserId { get; }

        /// <summary>
        /// Launches and starts the bot process with the provided update.
        /// </summary>
        /// <typeparam name="TUpdate">The type of trigger update that implements the <see cref="ISignedUpdate"/> interface.</typeparam>
        /// <param name="update">The update to launch the bot process with.</param>
        public Task LaunchWith<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate;
        /// <summary>
        /// Handles and processes input from the user with the provided update during the execution of the bot process.
        /// </summary>
        /// <typeparam name="TUpdate">The type of trigger update that implements the <see cref="ISignedUpdate"/> interface.</typeparam>
        /// <param name="update">The update to launch the bot process with.</param>
        public Task HandleInput<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate;
    }
}   