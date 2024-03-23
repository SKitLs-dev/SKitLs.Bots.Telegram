using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Interactions.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="IBotAction"/>&lt;<see cref="SignedMessageTextUpdate"/>&gt;,
    /// used for handling commands and executing associated actions.
    /// </summary>
    public class DefaultCommand : DefaultBotAction<SignedMessageTextUpdate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCommand"/> class with specific data.
        /// </summary>
        /// <param name="base">The base name for the action.</param>
        /// <param name="action">The action to be executed.</param>
        /// <exception cref="ArgumentNullException">Thrown when the base name or action is null.</exception>
        public DefaultCommand(string @base, BotInteraction<SignedMessageTextUpdate> action) : base(@base, action) { }

        /// <inheritdoc/>
        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => $"/{ActionNameBase}" == update.Text;
    }
}