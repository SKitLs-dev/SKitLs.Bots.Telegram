using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Defaults
{
    /// <summary>
    /// The <see cref="StringWrapper"/> class is a special wrapper used to encapsulate <see cref="string"/> data and
    /// integrate it into the <see cref="IArgedAction{TArg, TUpdate}"/> interface as a serializable argument.
    /// <para>
    /// It is a specific solution designed as an implementation of the <see cref="ArgumentWrapper{T}"/> for strings.
    /// </para>
    /// </summary>
    public class StringWrapper : ArgumentWrapper<string>
    {
        /// <summary>
        /// Creates a new instance of <see cref="StringWrapper"/> with default data.
        /// </summary>
        public StringWrapper() : base(string.Empty) { }
        /// <summary>
        /// Creates a new instance of <see cref="StringWrapper"/> with specific data.
        /// </summary>
        /// <param name="value">Value of the holding data.</param>
        public StringWrapper(string value) : base(value) { }

        /// <summary>
        /// implicit operator for StringWrapper => string
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator StringWrapper(string value) => new(value);
        /// <summary>
        /// implicit operator for string => StringWrapper
        /// </summary>
        /// <param name="source"></param>
        public static implicit operator string(StringWrapper source) => source.Value;
    }
}