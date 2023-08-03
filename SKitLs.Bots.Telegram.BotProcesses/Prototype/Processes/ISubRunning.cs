namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    /// <summary>
    /// Represents an interface for a running sub-process within a parent bot running process.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner (parent) bot running process, implementing <see cref="IBotRunningProcess"/>.</typeparam>
    public interface ISubRunning<TOwner> where TOwner : IBotRunningProcess
    {
        /// <summary>
        /// Represents the order of the sub-process within the parent bot running process.
        /// </summary>
        public int SubOrder { get; }

        /// <summary>
        /// Represents the owner (parent) bot running process associated with this sub-process.
        /// </summary>
        public TOwner Owner { get; }
    }
}