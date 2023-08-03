using SKitLs.Bots.Telegram.BotProcesses.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Partial
{
    /// <summary>
    /// Represents a generic class for wrapping an argument of a <see cref="PartialInputProcess{TResult}"/> bot process,
    /// implementing the <see cref="IProcessArgument"/> interface.
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    public class PartialInputArgument<TResult> : IProcessArgument
    {
        /// <summary>
        /// Represents the wrapped argument value.
        /// </summary>
        public TResult BuildingInstance { get; set; }

        /// <summary>
        /// Represents the completion status of the process associated with this argument.
        /// The default value is <see cref="ProcessCompleteStatus.Pending"/>.
        /// </summary>
        public ProcessCompleteStatus CompleteStatus { get; set; } = ProcessCompleteStatus.Pending;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialInputArgument{TResult}"/> class with the specified value.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        public PartialInputArgument(TResult value) => BuildingInstance = value;
    }
}