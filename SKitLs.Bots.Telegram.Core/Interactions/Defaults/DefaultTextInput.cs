using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Interactions.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="IBotAction"/>&lt;<see cref="SignedMessageTextUpdate"/>&gt;,
    /// used for handling text inputs and executing associated actions.
    /// </summary>
    public class DefaultTextInput : DefaultBotAction<SignedMessageTextUpdate>
    {
        /// <summary>
        /// Determines whether the case of the input string should be ignored.
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTextInput"/> class with specific data.
        /// </summary>
        /// <param name="base">The base name for the action.</param>
        /// <param name="action">The action to be executed.</param>
        /// <param name="ignoreCase">Determines whether the action is case sensitive.</param>
        /// <exception cref="ArgumentNullException">Thrown when the base name or action is null.</exception>
        public DefaultTextInput(string @base, BotInteraction<SignedMessageTextUpdate> action, bool ignoreCase = true) : base(@base, action) => IgnoreCase = ignoreCase;

        /// <inheritdoc/>
        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => IgnoreCase
            ? ActionNameBase.Equals(update.Text, StringComparison.CurrentCultureIgnoreCase)
            : update.Text == ActionNameBase;
    }
}