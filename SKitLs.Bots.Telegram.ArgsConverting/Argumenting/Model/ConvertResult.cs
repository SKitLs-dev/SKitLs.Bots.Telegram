using SKitLs.Bots.Telegram.Core.Exceptions.Internal;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model
{
    /// <summary>
    /// Represents a convertation result for <see cref="ConvertRule{TOut}"/>.
    /// Contains resulting value or excpetion message.
    /// </summary>
    /// <typeparam name="Out">Целевой тип конвертации</typeparam>
    public class ConvertResult<TOut>
    {
        private TOut? _value;
        /// <summary>
        /// Represents the value that has been gotten in the result of <see cref="ConvertRule{TOut}"/> work.
        /// Throws Null Exception if value <see langword="null"/>.
        /// </summary>
        /// <exception cref="BotManagerExcpetion"></exception>
        public TOut Value
        {
            get => _value ?? throw new BotManagerExcpetion("ConvertNullValue");
            set => _value = value;
        }

        /// <summary>
        /// Represents a targeted result type.
        /// </summary>
        public Type ValueType => typeof(TOut);
        /// <summary>
        /// Represents convertation result type.
        /// </summary>
        public ConvertResultType ResultType { get; private set; }
        /// <summary>
        /// Contains a message that describes convertation result.
        /// </summary>
        public string ResultMessage { get; private set; }

        /// <summary>
        /// Creates a new instance of a type <see cref="ConvertResult"/> with a specified data.
        /// </summary>
        /// <param name="resultType">Convertation result type.</param>
        /// <param name="message">A message that describes convertation result.</param>
        public ConvertResult(ConvertResultType resultType, string? message = null)
        {
            ResultType = resultType;
            ResultMessage = message ?? nameof(resultType);
        }
        /// <summary>
        /// Creates a new instance of a type <see cref="ConvertResult"/> with a specified data.
        /// </summary>
        /// <param name="resultType">Convertation result type.</param>
        /// <param name="message">Message that describes convertation result.</param>
        /// <param name="value">Value that has been gotten in the result of convertation.</param>
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
        /// A shortcut for creating <see cref="ConvertResultType.NotDefinied"/> with custom message <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Custom message.</param>
        /// <returns><see cref="ConvertResult{TOut}"/> with <see cref="ConvertResultType.NotDefinied"/> status.</returns>
        internal static ConvertResult<TOut> NotDefined(string? message = null) => new(ConvertResultType.NotDefinied, message);
    }
}