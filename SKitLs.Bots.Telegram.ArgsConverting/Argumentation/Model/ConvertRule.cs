namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// An abstract class used for storing conversion rules for objects.
    /// Serves as the base class for <see cref="ConvertResult{Out}"/>.
    /// </summary>
    public abstract class ConvertRule
    {
        /// <summary>
        /// Represents the targeted result type for the conversion.
        /// </summary>
        public Type ResultType { get; private init; }

        /// <summary>
        /// Creates a new instance of the <see cref="ConvertRule"/> class with the specified result type.
        /// </summary>
        /// <param name="resultType">The targeted result type for the conversion.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resultType"/> is <see langword="null"/>.</exception>
        protected ConvertRule(Type resultType)
        {
            ResultType = resultType ?? throw new ArgumentNullException(nameof(resultType));
        }
    }
}