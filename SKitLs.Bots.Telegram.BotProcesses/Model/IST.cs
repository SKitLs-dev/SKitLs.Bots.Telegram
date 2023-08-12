using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model
{
    /// <summary>
    /// Represents a container for a triple of descriptive values related to arbitrary processes.
    /// </summary>
    public class IST
    {
        /// <summary>
        /// Represents the unique identifier for the process.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Represents the user state associated with the process.
        /// </summary>
        public IUserState State { get; set; }

        /// <summary>
        /// Represents the terminational key for the process.
        /// </summary>
        public string TerminationalKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IST"/> class with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the process.</param>
        /// <param name="state">The user state associated with the process.</param>
        /// <param name="terminationalKey">The terminational key for the process.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        public IST(string id, IUserState state, string terminationalKey)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            State = state ?? throw new ArgumentNullException(nameof(state));
            TerminationalKey = terminationalKey ?? throw new ArgumentNullException(nameof(terminationalKey));
        }

        /// <summary>
        /// <b>Shortcut.</b> Creates a dynamic <see cref="IST"/> instance that would be overridden during the runtime.
        /// </summary>
        /// <returns>A new instance of <see cref="IST"/> representing a dynamic values.</returns>
        public static IST Dynamic() => new("process.dynamic", new DefaultUserState(-100, "dynamic"), "dynamic");
    }
}