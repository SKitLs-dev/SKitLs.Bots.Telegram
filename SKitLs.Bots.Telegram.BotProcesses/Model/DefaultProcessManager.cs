using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Exceptions.External;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model
{
    /// <summary>
    /// Default implementation of the <see cref="IProcessManager"/> interface for managing bot processes within the application.
    /// </summary>
    public class DefaultProcessManager : IProcessManager
    {
        /// <summary>
        /// Gets or sets the dictionary of defined bot processes with their unique process IDs as keys.
        /// </summary>
        /// <remarks>
        /// The DefinedProcesses dictionary holds a collection of bot processes that have been defined and registered within the process manager.
        /// The unique process IDs act as keys, allowing quick and efficient access to specific processes based on their IDs.
        /// Developers can add, remove, or modify bot processes in this dictionary, effectively defining the available processes within the application.
        /// </remarks>
        private Dictionary<string, IBotProcess> DefinedProcesses { get; set; } = new();
        /// <summary>
        /// Gets or sets the dictionary of running bot processes with the user's unique identifiers as keys.
        /// </summary>
        /// <remarks>
        /// The RunningProcesses dictionary contains a collection of currently running bot processes associated with individual users.
        /// The user's unique identifiers serve as keys, facilitating rapid retrieval and management of running processes for specific users.
        /// Developers can add, remove, or modify running processes in this dictionary as users initiate or terminate bot processes.
        /// </remarks>
        private Dictionary<long, IBotRunningProcess> RunningProcesses { get; set; } = new();

        /// <summary>
        /// Defines a single bot process and registers it within the process manager.
        /// </summary>
        /// <param name="process">The instance of the bot process to be defined and registered.</param>
        public void Define(IBotProcess process)
        {
            if (DefinedProcesses.ContainsKey(process.ProcessDefId))
                throw new DuplicationException(GetType(), typeof(IBotProcess), process.ProcessDefId);
            DefinedProcesses.Add(process.ProcessDefId, process);
        }
        /// <summary>
        /// Defines a collection of bot processes and registers them within the process manager.
        /// </summary>
        /// <param name="processes">The collection of bot processes to be defined and registered.</param>
        public void Define(ICollection<IBotProcess> processes) => processes.ToList().ForEach(x => Define(x));

        /// <summary>
        /// Retrieves the bot process instance that has been previously defined (<see cref="Define(IBotProcess)"/>)
        /// based on its unique process Id.
        /// </summary>
        /// <param name="processId">The unique identifier of the bot process to be retrieved.</param>
        /// <returns>The bot process instance with the specified Id, or throws an exception if the process is not found.</returns>
        /// <exception cref="NotDefinedException"></exception>
        public IBotProcess GetDefined(string processId) => DefinedProcesses.GetValueOrDefault(processId)
            ?? throw new NotDefinedException(GetType(), typeof(IBotProcess), processId);

        /// <summary>
        /// Runs a bot process with the provided arguments and information about the user.
        /// </summary>
        /// <typeparam name="T">The type of process arguments that implement the <see cref="IProcessArgument"/> interface.</typeparam>
        /// <param name="process">The bot process instance to be executed.</param>
        /// <param name="args">The arguments to be passed to the process during its execution.</param>
        /// <param name="update">The update received from the bot, containing relevant user information.</param>
        /// <returns>The running bot process instance representing the ongoing execution of the process.</returns>
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, ISignedUpdate update) where T : IProcessArgument
            => update.Sender is IStatefulUser stateful ? Run(process, args, stateful) : throw new NotStatefulException(this);
        /// <summary>
        /// Runs a bot process with the provided arguments and information about the user.
        /// </summary>
        /// <typeparam name="T">The type of process arguments that implement the <see cref="IProcessArgument"/> interface.</typeparam>
        /// <param name="process">The bot process instance to be executed.</param>
        /// <param name="args">The arguments to be passed to the process during its execution.</param>
        /// <param name="sender">The user who initiated the process execution.</param>
        /// <returns>The running bot process instance representing the ongoing execution of the process.</returns>
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, IStatefulUser sender) where T : IProcessArgument
        {
            var running = process.GetRunning(sender.TelegramId, args);
            sender.State = process.ProcessState;
            RunningProcesses.Add(running.OwnerUserId, running);
            return running;
        }

        /// <summary>
        /// Retrieves the running bot process associated with the specified user.
        /// </summary>
        /// <param name="sender">The user for whom the running bot process is being sought.</param>
        /// <returns>The running bot process associated with the user, or <see langword="null"/> if no such process is found.</returns>
        public IBotRunningProcess? GetRunning(IStatefulUser sender) => RunningProcesses.GetValueOrDefault(sender.TelegramId);
        /// <summary>
        /// Terminates the execution of a bot process for the specified user.
        /// </summary>
        /// <param name="sender">The unique identifier of the user for whom the bot process will be terminated.</param>
        public void Terminate(IStatefulUser sender)
        {
            sender.ResetState();
            if (RunningProcesses.ContainsKey(sender.TelegramId))
                RunningProcesses.Remove(sender.TelegramId);
        }
    }
}