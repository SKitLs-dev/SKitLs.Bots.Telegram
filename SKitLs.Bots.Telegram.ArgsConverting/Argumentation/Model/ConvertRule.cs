namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// An abstract class used for storing conversion rules for objects.
    /// Serves as the base class for <see cref="ConvertResult{Out}"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ConvertRule"/> class with the specified result type.
    /// </remarks>
    /// <param name="resultType">The targeted result type for the conversion.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="resultType"/> is <see langword="null"/>.</exception>
    public abstract class ConvertRule(Type resultType)
    {
        /// <summary>
        /// Gets the targeted result type for the conversion.
        /// </summary>
        public Type ResultType { get; private init; } = resultType ?? throw new ArgumentNullException(nameof(resultType));

        /// <summary>
        /// Converts the input string to the specified result type.
        /// </summary>
        /// <param name="input">The input string to be converted.</param>
        /// <returns>A <see cref="ConvertResult"/> containing the result of the conversion.</returns>
        public abstract ConvertResult Convert(string input);
    }
}