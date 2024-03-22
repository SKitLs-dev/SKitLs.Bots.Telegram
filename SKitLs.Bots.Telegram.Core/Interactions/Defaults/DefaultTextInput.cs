﻿using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
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
        public DefaultTextInput(string @base, BotInteraction<SignedMessageTextUpdate> action, bool ignoreCase = true)
            : base(@base, action) => IgnoreCase = ignoreCase;

        /// <summary>
        /// UNSAFE. Initializes a new instance of the <see cref="DefaultTextInput"/> class with specific data.
        /// Use this constructor to avoid compiler errors when passing non-static methods
        /// to the base() constructor for an action.
        /// <para>Do not forget to override the <see cref="DefaultBotAction{TUpdate}.Action"/> property.</para>
        /// </summary>
        /// <param name="base">The base name for the action.</param>
        /// <param name="ignoreCase">Determines whether the action is case sensitive.</param>
        /// <exception cref="ArgumentNullException">Thrown when the base name is null.</exception>
        [Obsolete("Do not forget to override the Action property")]
        protected DefaultTextInput(string @base, bool ignoreCase = true)
            : base(@base) => IgnoreCase = ignoreCase;

        /// <inheritdoc/>
        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => IgnoreCase
            ? ActionNameBase.Equals(update.Text, StringComparison.CurrentCultureIgnoreCase)
            : update.Text == ActionNameBase;
    }
}