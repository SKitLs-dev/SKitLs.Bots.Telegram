using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    // XML-Doc Update
    /// <summary>
    /// Default realization of <see cref="IBotAction"/>&lt;<see cref="SignedMessageTextUpdate"/>&gt;
    /// used for handling text inputs and executing them.
    /// </summary>
    public class DefaultTextInput : DefaultBotAction<SignedMessageTextUpdate>
    {
        /// <summary>
        /// Determines whether case of input string should be ignored.
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="DefaultTextInput"/> with specific data.
        /// </summary>
        /// <param name="base">Action name base.</param>
        /// <param name="action">An action to be executed.</param>
        /// <param name="ignoreCase">Determines whether action is case sensitive.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefaultTextInput(string @base, BotInteraction<SignedMessageTextUpdate> action, bool ignoreCase = true)
            : base(@base, action) => IgnoreCase = ignoreCase;

        /// <summary>
        /// UNSAFE. Creates a new instance of a <see cref="DefaultTextInput"/>
        /// with specific data. Use to avoid compiler errors when passing non-static methods
        /// to base() constructor for an action.
        /// <para>Do not forget to override <see cref="DefaultBotAction{TUpdate}.Action"/> property.</para>
        /// </summary>
        /// <param name="base">Action name base.</param>
        /// <param name="ignoreCase">Determines whether action is case sensitive.</param>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("Do not forget to override Action property")]
        protected DefaultTextInput(string @base, bool ignoreCase = true)
            : base(@base) => IgnoreCase = ignoreCase;

        /// <summary>
        /// Checks either this action should be executed on a certain incoming update.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns><see langword="true"/> if this action should be executed; otherwise, <see langword="false"/>.</returns>
        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => IgnoreCase
            ? ActionNameBase.ToLower() == update.Text.ToLower()
            : ActionNameBase == update.Text;
    }
}