using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using System.Globalization;

namespace SKitLs.Bots.Telegram.Core.Interactions.Defaults
{
    /// <summary>
    /// Abstract base class representing a default bot action for processing updates of type <typeparamref name="TUpdate"/>.
    /// Implements interfaces <see cref="IFormattable"/> and <see cref="IBotAction{TUpdate}"/>.
    /// <para>
    /// The <see cref="DefaultBotAction{TUpdate}"/> class provides a solid foundation for defining
    /// and managing default bot actions in a bot application.
    /// It can be extended and implemented to support specific actions tailored to the application's needs.
    /// </para>
    /// </summary>
    /// <typeparam name="TUpdate">The type of update that the bot action processes, which must implement <see cref="ICastedUpdate"/>.</typeparam>
    public abstract class DefaultBotAction<TUpdate> : IFormattable, IBotAction<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// String that determines action's unique name base.
        /// Used to determine whether it should be executed on a certain update.
        /// <para>
        /// Example: for <c>/start</c> command <c>start</c> is an <see cref="ActionNameBase"/>.
        /// </para>
        /// </summary>
        public virtual string ActionNameBase { get; init; }

        /// <inheritdoc/>
        public virtual string ActionId => ActionNameBase;

        /// <inheritdoc/>
        public BotInteraction<TUpdate> Action { get; protected set; }

        /// <summary>
        /// Creates a new instance of an abstract <see cref="DefaultBotAction{TUpdate}"/> with specific data.
        /// </summary>
        /// <param name="base">Action name base.</param>
        /// <param name="action">An action to be executed.</param>
        /// <exception cref="ArgumentNullException">Thrown when the base name or action is null.</exception>
        public DefaultBotAction(string @base, BotInteraction<TUpdate> action)
        {
            ActionNameBase = @base ?? throw new ArgumentNullException(nameof(@base));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <inheritdoc/>
        public string GetSerializedData(params string[] args) => ActionNameBase;

        /// <inheritdoc/>
        public abstract bool ShouldBeExecutedOn(TUpdate update);

        /// <inheritdoc/>
        public bool Equals(IBotAction<TUpdate>? other)
        {
            if (other is null) return false;
            if (other is DefaultBotAction<TUpdate> defaultAction)
                return ActionNameBase == defaultAction.ActionNameBase;
            return false;
        }

        /// <inheritdoc/>
        public override string ToString() => ToString("D");

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">The format to use or <see langword="null"/> reference to use the default format.</param>
        /// <returns>The value of the current instance in the specified format.</returns>
        /// <exception cref="FormatException"></exception>
        public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">The format to use or <see langword="null"/> reference to use the default format.</param>
        /// <param name="provider">The provider to use to format the value or <see langword="null"/> reference
        /// to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>The value of the current instance in the specified format.</returns>
        /// <exception cref="FormatException"></exception>
        public string ToString(string? format, IFormatProvider? provider)
        {
            if (string.IsNullOrEmpty(format)) format = "D";
            //provider ??= CultureInfo.CurrentCulture;

            return format.ToUpperInvariant() switch
            {
                "D" => $"[{GetType().Name}] {ActionId}",
                "C" => $"/{ActionNameBase}",
                _ => throw new FormatException(string.Format("The {0} format string is not supported.", format)),
            };
        }
    }
}