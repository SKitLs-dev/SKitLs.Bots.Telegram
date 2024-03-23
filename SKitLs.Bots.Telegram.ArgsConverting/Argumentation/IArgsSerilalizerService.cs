using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.Core.Services;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation
{
    /// <summary>
    /// Determines serialization service interface that provides methods
    /// to serialize and deserialize method arguments, using Converting System Rules.
    /// <para>
    /// <seealso cref="ConvertRule{TOut}"/>, <seealso cref="ConvertResult{TOut}"/>
    /// </para>
    /// </summary>
    public interface IArgsSerializeService : IBotService
    {
        /// <summary>
        /// Gets the conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>. If doesn't exist - exception is raised.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>The conversion rule for <typeparamref name="TOut"/> type.</returns>
        public ConvertRule<TOut> ResolveTypeRule<TOut>() where TOut : notnull;

        /// <summary>
        /// Tries to get the conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>The conversion rule for <typeparamref name="TOut"/> type.</returns>
        public ConvertRule<TOut>? FindRule<TOut>() where TOut : notnull;

        /// <summary>
        /// Determines whether a conversion rule <see cref="ConvertResult{TOut}"/> is defined for the specified
        /// output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns><see langword="true"/> if requested rule is determined. Otherwise <see langword="false"/>.</returns>
        public bool IsDefined<TOut>() where TOut : notnull => ResolveTypeRule<TOut>() is not null;

        /// <summary>
        /// Adds new conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>.
        /// <para>
        /// Attempting to add a rule for existing without an override flag will result in an error.
        /// </para>
        /// </summary>
        /// <typeparam name="TOut">Type of the converter.</typeparam>
        /// <param name="rule">Rule to be added.</param>
        /// <param name="override">Determines whether rule should be overridden, if one exists.</param>
        public void AddRule<TOut>(ConvertRule<TOut> rule, bool @override = false) where TOut : notnull;

        /// <summary>
        /// Clears all defined conversion rules.
        /// </summary>
        public void Clear();

        /// <summary>
        /// Deserializes the specified input string to the output type <typeparamref name="TOut"/>,
        /// creating a new instance of a type <typeparamref name="TOut"/>.
        /// <typeparamref name="TOut"/> requires a parameterless constructor.
        /// </summary>
        /// <typeparam name="TOut">An output type, which must not be nullable and have a parameterless constructor.</typeparam>
        /// <param name="input">An input argument string.</param>
        /// <param name="splitToken">A token that the data is separated with.</param>
        /// <returns>The conversion result.</returns>
        public ConvertResult<TOut> Deserialize<TOut>(string input, char splitToken) where TOut : notnull, new();
        /// <summary>
        /// Deserializes the specified input string to the output type <typeparamref name="TOut"/>,
        /// overriding data of an existing <paramref name="instance"/>.
        /// </summary>
        /// <typeparam name="TOut">An output type.</typeparam>
        /// <param name="input">An input argument string.</param>
        /// <param name="instance">An instance that the deserialized data will be written to.</param>
        /// <param name="splitToken">A token that the data is separated with.</param>
        /// <returns>The conversion result.</returns>
        public ConvertResult<TOut> DeserializeTo<TOut>(string input, TOut instance, char splitToken) where TOut : notnull;
        /// <summary>
        /// Serializes the specified input object.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="input">Input argument string.</param>
        /// <param name="splitToken">Represents a token that the data is separated with.</param>
        /// <returns>A string that represents serialized <paramref name="input"/>.</returns>
        public string Serialize<TIn>(TIn input, char splitToken) where TIn : notnull;

        /// <summary>
        /// Creates a new object of a type <typeparamref name="TOut"/>, converting it from <paramref name="input"/> data
        /// with help of appropriate <see cref="ConvertRule{TOut}"/>.
        /// </summary>
        /// <typeparam name="TOut">A type of an object that should be unpacked.</typeparam>
        /// <param name="input">String data that should be converted to an instance of a type <typeparamref name="TOut"/>.</param>
        /// <returns><see cref="ConvertResult{TOut}"/> that contains information about converting attempt.</returns>
        public ConvertResult<TOut> Unpack<TOut>(string input) where TOut : notnull;
        /// <summary>
        /// Converts an object of a type <typeparamref name="TIn"/> to a string that could be used as a representation of
        /// this object.
        /// </summary>
        /// <typeparam name="TIn">A type of an object that should be packed.</typeparam>
        /// <param name="input">.</param>
        /// <returns>A string that could be used as a representation of <paramref name="input"/> object.</returns>
        public string Pack<TIn>(TIn input);
    }
}