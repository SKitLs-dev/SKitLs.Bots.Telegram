namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// An abstract class used for storing converting rules for objects.
    /// Used as a base for <see cref="ConvertResult{Out}"/>
    /// </summary>
    public abstract class ConvertRule
    {
        /// <summary>
        /// Represents a targeted result type.
        /// </summary>
        public Type ResultType { get; private init; }

        /// <summary>
        /// Creates a new instance of a <see cref="ConvertRule"/> with a specified data.
        /// </summary>
        /// <param name="resultType">Represents a targeted result type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resultType"/> is <see langword="null"/></exception>
        protected ConvertRule(Type resultType)
        {
            ResultType = resultType ?? throw new ArgumentNullException(nameof(resultType));
        }
    }
}