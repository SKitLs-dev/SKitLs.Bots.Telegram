using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting
{
    /// <summary>
    /// Determines serialization service interface that provides methods
    /// to serialize and deserialize method arguments, using Converting System Rules.
    /// <para>
    /// <seealso cref="ConvertRule{TOut}"/>, <seealso cref="ConvertResult{TOut}"/>
    /// </para>
    /// </summary>
    public interface IArgsSerilalizerService
    {
        /// <summary>
        /// Gets the conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>. If doesn't exist - exception is raised.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>The conversion rule for <typeparamref name="TOut"/> type.</returns>
        public ConvertRule<TOut> ResolveTypeRule<TOut>();

        /// <summary>
        /// Tries to get the conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>The conversion rule for <typeparamref name="TOut"/> type.</returns>
        public ConvertRule<TOut>? FindRule<TOut>();

        /// <summary>
        /// Determines whether a conversion rule <see cref="ConvertResult{TOut}"/> is defined for the specified
        /// output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>True if requested rule is determined. Otherwise False.</returns>
        public bool IsDefined<TOut>() => ResolveTypeRule<TOut>() is not null;

        /// <summary>
        /// Adds new conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>.
        /// <para>
        /// Attempting to add a rule for existing without an override flag will result in an error.
        /// </para>
        /// </summary>
        /// <typeparam name="TOut">Type of the converter.</typeparam>
        /// <param name="rule">Rule to be added.</param>
        /// <param name="override">If already exists, should be overriden.</param>
        public void AddRule<TOut>(ConvertRule<TOut> rule, bool @override = false);

        /// <summary>
        /// Clears all defined conversion rules.
        /// </summary>
        public void Clear();

        /// <summary>
        /// Deserializes the specified input string to the output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Output type.</typeparam>
        /// <param name="input">Input argument string.</param>
        /// <returns>Conversion result.</returns>
        public ConvertResult<TOut> Deserialize<TOut>(string input, char splitToken) where TOut : notnull, new();
        /// <summary>
        /// Serializes the specified input object.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Serialize<TIn>(TIn input, char splitToken) where TIn : notnull;

        public ConvertResult<TOut> Unpack<TOut>(string input) where TOut : notnull;
        public string Pack<TIn>(TIn input);
    }
}