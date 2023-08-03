using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions;
using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// Represents a converting result for <see cref="ConvertRule{TOut}"/>.
    /// Contains resulting value or exception message.
    /// </summary>
    /// <typeparam name="TOut">Target type of converting.</typeparam>
    public class ConvertResult<TOut> where TOut : notnull
    {
        private TOut? _value;
        /// <summary>
        /// Represents the value that has been gotten in the result of <see cref="ConvertRule{TOut}"/> work.
        /// Throws Null Exception if value <see langword="null"/>.
        /// </summary>
        /// <exception cref="ArgedInterException"></exception>
        public TOut Value
        {
            get => _value ?? throw new ArgedInterException("ConvertNullValue", SKTEOriginType.Inexternal, this);
            set => _value = value;
        }

        /// <summary>
        /// Represents a targeted result type.
        /// </summary>
        public Type ValueType => typeof(TOut);
        /// <summary>
        /// Represents converting result type.
        /// </summary>
        public ConvertResultType ResultType { get; private init; }
        /// <summary>
        /// Contains a message that describes converting result.
        /// </summary>
        public string ResultMessage { get; private init; }

        /// <summary>
        /// Creates a new instance of a type <see cref="ConvertResult{TOut}"/> with a specified data.
        /// </summary>
        /// <param name="resultType">Converting result type.</param>
        /// <param name="message">A message that describes converting result.</param>
        public ConvertResult(ConvertResultType resultType, string? message = null)
        {
            ResultType = resultType;
            ResultMessage = message ?? nameof(resultType);
        }
        /// <summary>
        /// Initializes a new instance of a type <see cref="ConvertResult{TOut}"/> with a specified data.
        /// </summary>
        /// <param name="resultType">Converting result type.</param>
        /// <param name="message">Message that describes converting result.</param>
        /// <param name="value">Value that has been gotten in the result of converting.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConvertResult(TOut value, ConvertResultType resultType = ConvertResultType.Ok, string? message = null)
            : this(resultType, message) => Value = value ?? throw new ArgumentNullException(nameof(value));

        /// <summary>
        /// A shortcut for creating <see cref="ConvertResultType.Ok"/> with <paramref name="value"/> result
        /// and custom message <paramref name="message"/>.
        /// </summary>
        /// <param name="value">Result value.</param>
        /// <param name="message">Custom message.</param>
        /// <returns><see cref="ConvertResult{TOut}"/> with <see cref="ConvertResultType.Ok"/> status.</returns>
        public static ConvertResult<TOut> OK(TOut value, string? message = null) => new(value, message: message);

        /// <summary>
        /// A shortcut for creating <see cref="ConvertResultType.NullInput"/> with custom message <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Custom message.</param>
        /// <returns><see cref="ConvertResult{TOut}"/> with <see cref="ConvertResultType.NullInput"/> status.</returns>
        public static ConvertResult<TOut> NullInput(string? message = null) => new(ConvertResultType.NullInput, message);

        /// <summary>
        /// A shortcut for creating <see cref="ConvertResultType.Incorrect"/> with custom message <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Custom message.</param>
        /// <returns><see cref="ConvertResult{TOut}"/> with <see cref="ConvertResultType.Incorrect"/> status.</returns>
        public static ConvertResult<TOut> Incorrect(string? message = null) => new(ConvertResultType.Incorrect, message);

        /// <summary>
        /// A shortcut for creating <see cref="ConvertResultType.NotPresented"/> with custom message <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Custom message.</param>
        /// <returns><see cref="ConvertResult{TOut}"/> with <see cref="ConvertResultType.NotPresented"/> status.</returns>
        public static ConvertResult<TOut> NotPresented(string? message = null) => new(ConvertResultType.NotPresented, message);

        /// <summary>
        /// A shortcut for creating <see cref="ConvertResultType.NotDefined"/> with custom message <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Custom message.</param>
        /// <returns><see cref="ConvertResult{TOut}"/> with <see cref="ConvertResultType.NotDefined"/> status.</returns>
        internal static ConvertResult<TOut> NotDefined(string? message = null) => new(ConvertResultType.NotDefined, message);
    }
}