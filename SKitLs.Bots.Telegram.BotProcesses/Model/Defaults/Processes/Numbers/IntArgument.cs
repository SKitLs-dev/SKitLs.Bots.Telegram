using SKitLs.Bots.Telegram.BotProcesses.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Numbers
{
    /// <summary>
    /// Represents a generic class for wrapping an argument of a <see cref="IntInputProcess"/> bot process,
    /// implementing the <see cref="IProcessArgument"/> interface.
    /// </summary>
    public class IntArgument : IProcessArgument
    {
        /// <summary>
        /// Represents the wrapped argument value.
        /// </summary>
        public int BuildingInstance { get; set; }
        /// <summary>
        /// Represents the completion status of the process associated with this argument.
        /// The default value is <see cref="ProcessCompleteStatus.Pending"/>.
        /// </summary>
        public ProcessCompleteStatus CompleteStatus { get; set; } = ProcessCompleteStatus.Pending;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntArgument"/> class with default data.
        /// </summary>
        public IntArgument() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="IntArgument"/> class with the specified value.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        public IntArgument(int value) => BuildingInstance = value;

        /// <summary>
        /// Converts <see cref="IntArgument"/> to <see cref="int"/> using its <see cref="BuildingInstance"/>.
        /// </summary>
        /// <param name="argument">Argument to be unpacked.</param>
        public static implicit operator int(IntArgument argument) => argument.BuildingInstance;
        /// <summary>
        /// Converts <see cref="int"/> to <see cref="IntArgument"/> assigning it to argument's <see cref="BuildingInstance"/>.
        /// </summary>
        /// <param name="value">Value to be packed.</param>
        public static implicit operator IntArgument(int value) => new(value);
    }
}