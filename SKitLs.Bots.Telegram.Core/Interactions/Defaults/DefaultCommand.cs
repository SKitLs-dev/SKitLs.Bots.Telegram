using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
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

        /// <summary>
        /// UNSAFE. Initializes a new instance of the <see cref="DefaultCommand"/> class with specific data.
        /// Use this constructor to avoid compiler errors when passing non-static methods
        /// to the base() constructor for an action.
        /// <para>Do not forget to override the <see cref="DefaultBotAction{TUpdate}.Action"/> property.</para>
        /// </summary>
        /// <param name="base">The base name for the action.</param>
        /// <exception cref="ArgumentNullException">Thrown when the base name is null.</exception>
        [Obsolete("Do not forget to override Action property")]
        protected DefaultCommand(string @base) : base(@base) { }

        /// <inheritdoc/>
        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => $"/{ActionNameBase}" == update.Text;
    }
}