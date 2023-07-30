using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using System.Globalization;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    public abstract class DefaultBotAction<TUpdate> : IFormattable, IBotAction<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// String that determines action's unique name base.
        /// Used to determine whether it should be executed on a certain update.
        /// <para>
        /// Example: for <c>/start</c> command <c>start</c> is an <see cref="ActionNameBase"/>.
        /// </para>
        /// </summary>
        public virtual string ActionNameBase { get; private set; }
        public virtual string ActionId => ActionNameBase;
        public BotInteraction<TUpdate> Action { get; protected set; }

        /// <summary>
        /// Creates a new instance of an abstract <see cref="DefaultBotAction{TUpdate}"/> with specific data.
        /// </summary>
        /// <param name="base">Action name base</param>
        /// <param name="action">An action to be executed</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefaultBotAction(string @base, BotInteraction<TUpdate> action)
        {
            ActionNameBase = @base ?? throw new ArgumentNullException(nameof(@base));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// UNSAFE. Creates a new instance of an abstract <see cref="DefaultBotAction{TUpdate}"/>
        /// with specific data. Use to avoid compiler errors when passing non-static methods
        /// to base() constructor for an action.
        /// <para>Do not forget to override <see cref="Action"/> property.</para>
        /// </summary>
        /// <param name="base">Action name base</param>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("Do not forget to override Action property")]
        protected DefaultBotAction(string @base)
        {
            ActionNameBase = @base ?? throw new ArgumentNullException(nameof(@base));
            Action = null!;
        }

        public string GetSerializedData(params string[] args) => ActionNameBase;
        public abstract bool ShouldBeExecutedOn(TUpdate update);

        public bool Equals(IBotAction<TUpdate>? other)
        {
            if (other is null) return false;
            if (other is DefaultBotAction<TUpdate> defaultAction)
                return ActionNameBase == defaultAction.ActionNameBase;

            return false;
        }

        public override string ToString() => ToString("D");

        public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);
        public string ToString(string? format, IFormatProvider? provider)
        {
            if (string.IsNullOrEmpty(format)) format = "D";
            //provider ??= CultureInfo.CurrentCulture;

            return format.ToUpperInvariant() switch
            {
                "D" => $"[{GetType().Name}] {ActionId}",
                "C" => $"/{ActionNameBase}",
                _ => throw new FormatException(String.Format("The {0} format string is not supported.", format)),
            };
        }
    }
}