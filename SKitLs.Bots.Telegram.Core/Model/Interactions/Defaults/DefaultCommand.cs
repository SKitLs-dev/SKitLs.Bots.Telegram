using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    // XML-Doc Update
    /// <summary>
    /// Default realization of <see cref="IBotAction"/>&lt;<see cref="SignedMessageTextUpdate"/>&gt;
    /// used for handling commands and executing them.
    /// </summary>
    public class DefaultCommand : DefaultBotAction<SignedMessageTextUpdate>
    {
        /// <summary>
        /// Creates a new instance of a <see cref="DefaultCommand"/> with specific data.
        /// </summary>
        /// <param name="base">Action name base.</param>
        /// <param name="action">An action to be executed.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefaultCommand(string @base, BotInteraction<SignedMessageTextUpdate> action) : base(@base, action) { }

        /// <summary>
        /// UNSAFE. Creates a new instance of a <see cref="DefaultCommand"/>
        /// with specific data. Use to avoid compiler errors when passing non-static methods
        /// to base() constructor for an action.
        /// <para>Do not forget to override <see cref="DefaultBotAction{TUpdate}.Action"/> property.</para>
        /// </summary>
        /// <param name="base">Action name base.</param>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("Do not forget to override Action property")]
        protected DefaultCommand(string @base) : base(@base) { }

        /// <summary>
        /// Checks either this action should be executed on a certain incoming update.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns><see langword="true"/> if this action should be executed; otherwise, <see langword="false"/>.</returns>
        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => $"/{ActionNameBase}" == update.Text;
    }
}