using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Stateful.Exceptions.Inexternal;
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

        /// <inheritdoc/>
        public void Define(IBotProcess process)
        {
            if (DefinedProcesses.ContainsKey(process.ProcessDefId))
                throw new DuplicationException(GetType(), typeof(IBotProcess), process.ProcessDefId);
            DefinedProcesses.Add(process.ProcessDefId, process);
        }

        /// <inheritdoc/>
        public void Define(ICollection<IBotProcess> processes) => processes.ToList().ForEach(x => Define(x));

        /// <inheritdoc/>
        /// <exception cref="NotDefinedException"></exception>
        public IBotProcess GetDefined(string processId) => DefinedProcesses.GetValueOrDefault(processId) ?? throw new NotDefinedException(GetType(), typeof(IBotProcess), processId);

        /// <inheritdoc/>
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, ISignedUpdate update) where T : IProcessArgument
            => update.Sender is IStatefulUser stateful ? Run(process, args, stateful) : throw new NotStatefulException(this);

        /// <inheritdoc/>
        public IBotRunningProcess Run<T>(IBotProcess<T> process, T args, IStatefulUser sender) where T : IProcessArgument
        {
            var running = process.GetRunning(sender.TelegramId, args);
            sender.State = process.ProcessState;
            RunningProcesses.Add(running.OwnerUserId, running);
            return running;
        }

        /// <inheritdoc/>
        public IBotRunningProcess? GetRunning(IStatefulUser sender) => RunningProcesses.GetValueOrDefault(sender.TelegramId);

        /// <inheritdoc/>
        public void Terminate(IStatefulUser sender)
        {
            sender.ResetState();
            if (RunningProcesses.ContainsKey(sender.TelegramId))
                RunningProcesses.Remove(sender.TelegramId);
        }
    }
}