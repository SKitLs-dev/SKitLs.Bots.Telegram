using SKitLs.Bots.Telegram.BotProcesses.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Shot
{
    /// <summary>
    /// Represents a generic class for wrapping an argument of a <see cref="ShotInputProcess{TResult}"/> bot process,
    /// implementing the <see cref="IProcessArgument"/> interface.
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    [Obsolete($"Replaced with {nameof(TextInputsArguments<TResult>)}", true)]
    public class ShotArgument<TResult> : IProcessArgument where TResult : notnull
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
        /// Initializes a new instance of the <see cref="ShotArgument{TResult}"/> class with the specified value.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        public ShotArgument(TResult value) => BuildingInstance = value;
    }
}