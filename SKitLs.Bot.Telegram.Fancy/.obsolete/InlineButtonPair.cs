using SKitLs.Bots.Telegram.Core.Model.Interactions;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    /// <summary>
    /// Represents specific data container that carries info about inline button: label, data, single line.
    /// </summary>
    [Obsolete("Obsolete. Replaced with 'InlineButton's")]
    public class InlineButtonPair
    {
        /// <summary>
        /// Represents callback's label.
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Represents callback's data.
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Determines either button should be printed on stand-alone single line.
        /// </summary>
        public bool SingleLine { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="InlineButtonPair"/> with specified data.
        /// </summary>
        /// <param name="labeled">Callback's labeled data.</param>
        public InlineButtonPair(LabeledData labeled) : this(labeled.Label, labeled.Data) { }
        /// <summary>
        /// Creates a new instance of <see cref="InlineButtonPair"/> with specified data.
        /// </summary>
        /// <param name="label">Button's label to display.</param>
        /// <param name="data">Button's callback data.</param>
        public InlineButtonPair(string label, string data)
        {
            Label = label;
            Data = data;
        }
    }
}