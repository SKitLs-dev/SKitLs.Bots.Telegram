namespace SKitLs.Bots.Telegram.BotProcesses.Prototype
{
    /// <summary>
    /// Represents an enumeration of process completion statuses.
    /// </summary>
    public enum ProcessCompleteStatus
    {
        /// <summary>
        /// Indicates that the process has failed.
        /// </summary>
        Failure = -1,

        /// <summary>
        /// Indicates that the process is pending and not yet completed.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Indicates that the process has been successfully completed.
        /// </summary>
        Success = 10,

        /// <summary>
        /// Indicates that the process has been canceled before completion.
        /// </summary>
        Canceled = 20,
    }
}