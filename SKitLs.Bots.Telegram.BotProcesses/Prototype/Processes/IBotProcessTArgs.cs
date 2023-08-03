namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    /// <summary>
    /// <see cref="IBotProcess{TArg}"/> is a generic interface that extends the <see cref="IBotProcess"/> interface
    /// and introduces a type parameter <typeparamref name="TArgs"/>, representing the specific arguments
    /// required for executing the bot process.
    /// </summary>
    /// <typeparam name="TArgs">The type of process arguments that implement the <see cref="IProcessArgument"/> interface.</typeparam>
    public interface IBotProcess<TArgs> : IBotProcess where TArgs : IProcessArgument
    {
        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the bot process is running.</param>
        /// <param name="args">The specific arguments required to execute the bot process.</param>
        /// <returns>The running bot process instance representing the ongoing execution of the process.</returns>
        public IBotRunningProcess GetRunning(long userId, TArgs args);
    }
}