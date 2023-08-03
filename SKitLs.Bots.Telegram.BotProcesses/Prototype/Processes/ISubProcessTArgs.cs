namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    /// <summary>
    /// Represents an interface for a running sub-process within a parent bot running process.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner (parent) bot running process, implementing <see cref="IBotRunningProcess"/>.</typeparam>
    /// <typeparam name="TArgs">The type of process arguments that implement the <see cref="IProcessArgument"/> interface.</typeparam>
    public interface ISubProcess<TOwner, TArgs> where TOwner : IBotRunningProcess where TArgs : IProcessArgument
    {
        /// <summary>
        /// Represents the order of the sub-process within the parent bot running process.
        /// </summary>
        public int SubOrder { get; }
        /// <summary>
        /// Determines whether this sub-process is terminational.
        /// </summary>
        public bool IsTerminational { get; }

        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="owner">Parent bot running process, that has raised running.</param>
        /// <param name="args">The specific arguments required to execute the bot process.</param>
        /// <returns>The running bot process instance representing the ongoing execution of the process.</returns>
        public ISubRunning<TOwner> GetRunning(TOwner owner, TArgs args);
    }
}