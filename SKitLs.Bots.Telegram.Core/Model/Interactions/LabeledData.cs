namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    // XML-Doc Update
    /// <summary>
    /// Represents a data structure for storing labeled information used in callbacks.
    /// </summary>
    public class LabeledData
    {
        /// <summary>
        /// Represents displaying label associated with the data.
        /// </summary>
        public string Label { get; init; }
        /// <summary>
        /// Represents data value. Data is sent to and received from server.
        /// </summary>
        public string Data { get; init; }

        /// <summary>
        /// Initializes a new instance of <see cref="LabeledData"/> class with the specified label and data.
        /// </summary>
        /// <param name="label">The label associated with the data.</param>
        /// <param name="data">The data value to be stored.</param>
        /// <exception cref="ArgumentNullException">Thrown when either label or data is null.</exception>
        public LabeledData(string label, string data)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}