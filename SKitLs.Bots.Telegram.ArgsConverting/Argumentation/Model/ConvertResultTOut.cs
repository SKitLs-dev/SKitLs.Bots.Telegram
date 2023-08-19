using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions;
using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// Represents a conversion result for the <see cref="ConvertRule{TOut}"/> class.
    /// Contains either the resulting value or an exception message.
    /// </summary>
    /// <typeparam name="TOut">The target type of the conversion.</typeparam>
    public class ConvertResult<TOut> : ConvertResult where TOut : notnull
    {
        private TOut? _value;
        /// <summary>
        /// Represents the value that has been obtained as a result of the <see cref="ConvertRule{TOut}"/> operation.
        /// Throws a <see cref="ArgedInterException"/> if the value is <see langword="null"/>.
        /// </summary>
        /// <exception cref="ArgedInterException">Thrown when the value is null.</exception>
        public TOut Value
        {
            get => _value ?? throw new ArgedInterException("ConvertNullValue", SKTEOriginType.Inexternal, this);
            set => _value = value;
        }

        /// <inheritdoc/>
        public override Type ValueType => typeof(TOut);

        /// <summary>
        /// Creates a new instance of the <see cref="ConvertResult{TOut}"/> class with the specified data.
        /// </summary>
        /// <param name="resultType">The type of the conversion result.</param>
        /// <param name="message">A message that describes the conversion result.</param>
        public ConvertResult(ConvertResultType resultType, string? message = null) : base(resultType, message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertResult{TOut}"/> class with the specified data.
        /// </summary>
        /// <param name="value">The value obtained from the conversion.</param>
        /// <param name="resultType">The type of the conversion result.</param>
        /// <param name="message">A message that describes the conversion result.</param>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        public ConvertResult(TOut value, ConvertResultType resultType = ConvertResultType.Ok, string? message = null)
            : this(resultType, message) => Value = value ?? throw new ArgumentNullException(nameof(value));

        public override object GetValue() => Value;

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