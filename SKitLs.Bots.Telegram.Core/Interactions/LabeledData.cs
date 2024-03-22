namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    /// <summary>
    /// Represents a data structure for storing labeled information used in callback interactions.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LabeledData"/> class with the specified label and data.
    /// </remarks>
    /// <param name="label">The label associated with the data.</param>
    /// <param name="data">The value of the data to be stored.</param>
    /// <exception cref="ArgumentNullException">Thrown when either label or data is null.</exception>
    public class LabeledData(string label, string data)
    {
        /// <summary>
        /// Gets the display label associated with the data.
        /// </summary>
        public string Label { get; init; } = label ?? throw new ArgumentNullException(nameof(label));

        /// <summary>
        /// Gets the value of the data associated with the label.
        /// </summary>
        public string Data { get; init; } = data ?? throw new ArgumentNullException(nameof(data));
    }
}