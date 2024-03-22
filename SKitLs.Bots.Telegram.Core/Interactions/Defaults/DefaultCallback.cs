using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="IBotAction"/>&lt;<see cref="SignedCallbackUpdate"/>&gt;,
    /// used for handling callbacks and executing associated actions.
    /// </summary>
    public class DefaultCallback : DefaultBotAction<SignedCallbackUpdate>
    {
        /// <summary>
        /// The label to be displayed for inline keyboards markup.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCallback"/> class with specific data of <see cref="LabeledData"/>.
        /// </summary>
        /// <param name="data">The specific labeled data pair.</param>
        /// <param name="action">The action to be executed.</param>
        /// <exception cref="ArgumentNullException">Thrown when the data or action is null.</exception>
        public DefaultCallback(LabeledData data, BotInteraction<SignedCallbackUpdate> action)
            : this(data.Data, data.Label, action) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCallback"/> class with specific data.
        /// </summary>
        /// <param name="base">The base name for the action.</param>
        /// <param name="label">The label to be displayed.</param>
        /// <param name="action">The action to be executed.</param>
        /// <exception cref="ArgumentNullException">Thrown when the base name, label, or action is null.</exception>
        public DefaultCallback(string @base, string label, BotInteraction<SignedCallbackUpdate> action)
            : base(@base, action) => Label = label;

        /// <summary>
        /// UNSAFE. Initializes a new instance of the <see cref="DefaultCallback"/> class with specific data.
        /// Use this constructor to avoid compiler errors when passing non-static methods
        /// to the base() constructor for an action.
        /// <para>Do not forget to override the <see cref="DefaultBotAction{TUpdate}.Action"/> property.</para>
        /// </summary>
        /// <param name="base">The base name for the action.</param>
        /// <param name="label">The label to be displayed.</param>
        /// <exception cref="ArgumentNullException">Thrown when the base name or label is null.</exception>
        [Obsolete("Do not forget to override Action property")]
        protected DefaultCallback(string @base, string label) : base(@base) => Label = label;

        /// <inheritdoc/>
        public override bool ShouldBeExecutedOn(SignedCallbackUpdate update) => ActionNameBase == update.Data;

        /// <inheritdoc/>
        public override string ToString() => $"[{GetType().Name}] \"{Label}\" - {ActionNameBase}";
    }
}