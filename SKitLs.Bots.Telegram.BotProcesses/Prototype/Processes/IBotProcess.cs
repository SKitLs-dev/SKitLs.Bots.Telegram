using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes
{
    /// <summary>
    /// <see cref="IBotProcess"/> interface representing a bot process.
    /// </summary>
    public interface IBotProcess
    {
        /// <summary>
        /// Represents the unique identifier of the bot process definition.
        /// It serves as a distinguishing identifier for the particular bot process within the application.
        /// </summary>
        public string ProcessDefId { get; }

        /// <summary>
        /// Represents the state associated with the bot process.
        /// Allows to maintain user-specific state information, enabling the resumption of interrupted processes
        /// or tracking progress.
        /// </summary>
        public IUserState ProcessState { get; }
    }
}