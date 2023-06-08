using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    /// <summary>
    /// Default realization of <see cref="IBotAction"/>&lt;<see cref="SignedCallbackUpdate"/>&gt;
    /// used for handiling callbacks and executing them.
    /// </summary>
    public class DefaultCallback : DefaultBotAction<SignedCallbackUpdate>
    {
        /// <summary>
        /// Display label for inline keyboards markup.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Creates a new instance of a <see cref="DefaultCallback"/> with specific data.
        /// </summary>
        /// <param name="base">Action name base</param>
        /// <param name="label">A label to be displayed</param>
        /// <param name="action">An action to be executed</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefaultCallback(string @base, string label, BotInteraction<SignedCallbackUpdate> action)
            : base(@base, action) => Label = label;

        /// <summary>
        /// UNSAFE. Creates a new instance of a <see cref="DefaultCallback"/>
        /// with specific data. Use to avoid compiler errors when passing non-static methods
        /// to base() constructor for an action.
        /// <para>Do not forget to override <see cref="DefaultBotAction{TUpdate}.Action"/> property.</para>
        /// </summary>
        /// <param name="base">Action name base</param>
        /// <param name="label">A label to be displayed</param>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("Do not forget to override Action property")]
        protected DefaultCallback(string @base, string label) : base(@base) => Label = label;

        public override bool ShouldBeExecutedOn(SignedCallbackUpdate update) => ActionNameBase == update.Data;

        public override string ToString() => $"[{GetType().Name}] \"{Label}\" - {ActionNameBase}";
    }
}