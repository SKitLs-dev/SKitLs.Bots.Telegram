using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype
{
    /// <summary>
    /// <see cref="IProcessManager"/> provides mechanics for managing bot processes within the application.
    /// </summary>
    public interface IProcessManager
    {
        /// <summary>
        /// Defines a single bot process and registers it within the process manager.
        /// </summary>
        /// <param name="process">The instance of the bot process to be defined and registered.</param>
        public void Define(IBotProcess process);

        /// <summary>
        /// Defines a collection of bot processes and registers them within the process manager.
        /// </summary>
        /// <param name="processes">The collection of bot processes to be defined and registered.</param>
        public void Define(ICollection<IBotProcess> processes);

        /// <summary>
        /// Retrieves the bot process instance that has been previously defined (<see cref="Define(IBotProcess)"/>)
        /// based on its unique process Id.
        /// </summary>
        /// <param name="processId">The unique identifier of the bot process to be retrieved.</param>
        /// <returns>The bot process instance with the specified Id.</returns>
        public IBotProcess GetDefined(string processId);

        /// <summary>
        /// Runs a bot process with the provided arguments and information about the user.
        /// </summary>
        /// <typeparam name="T">The type of process arguments that implement the <see cref="IProcessArgument"/> interface.</typeparam>
        /// <param name="process">The bot process instance to be executed.</param>
        /// <param name="args">The arguments to be passed to the process during its execution.</param>
        /// <param name="update">The update received from the bot, containing relevant user information.</param>
        /// <returns>The running bot process instance representing the ongoing execution of the process.</returns>
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, ISignedUpdate update) where T : IProcessArgument;

        /// <summary>
        /// Runs a bot process with the provided arguments and information about the user.
        /// </summary>
        /// <typeparam name="T">The type of process arguments that implement the <see cref="IProcessArgument"/> interface.</typeparam>
        /// <param name="process">The bot process instance to be executed.</param>
        /// <param name="args">The arguments to be passed to the process during its execution.</param>
        /// <param name="sender">The user who initiated the process execution.</param>
        /// <returns>The running bot process instance representing the ongoing execution of the process.</returns>
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, IStatefulUser sender) where T : IProcessArgument;

        /// <summary>
        /// Retrieves the running bot process associated with the specified user.
        /// </summary>
        /// <param name="sender">The user for whom the running bot process is being sought.</param>
        /// <returns>The running bot process associated with the user, or <see langword="null"/> if no such process is found.</returns>
        public IBotRunningProcess? GetRunning(IStatefulUser sender);

        /// <summary>
        /// Terminates the execution of a bot process for the specified user.
        /// </summary>
        /// <param name="sender">The unique identifier of the user for whom the bot process will be terminated.</param>
        public void Terminate(IStatefulUser sender);
    }
}