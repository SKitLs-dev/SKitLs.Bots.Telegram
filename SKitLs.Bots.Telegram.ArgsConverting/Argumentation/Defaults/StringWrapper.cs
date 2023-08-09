using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Defaults
{
    /// <summary>
    /// The <see cref="StringWrapper"/> class is a specialized wrapper designed to encapsulate <see cref="string"/> data and
    /// integrate it into the <see cref="IArgedAction{TArg, TUpdate}"/> interface as a serializable argument.
    /// <para>
    /// This class is a specific solution created as an implementation of the <see cref="ArgumentWrapper{T}"/> for strings.
    /// </para>
    /// </summary>
    public class StringWrapper : ArgumentWrapper<string>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="StringWrapper"/> class with default data.
        /// </summary>
        public StringWrapper() : base(string.Empty) { }
        /// <summary>
        /// Creates a new instance of the <see cref="StringWrapper"/> class with specific data.
        /// </summary>
        /// <param name="value">The value of the encapsulated data.</param>
        public StringWrapper(string value) : base(value) { }

        /// <summary>
        /// Defines an implicit operator for converting <see cref="StringWrapper"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="StringWrapper"/> instance to convert.</param>
        public static implicit operator StringWrapper(string value) => new(value);
        /// <summary>
        /// Defines an implicit operator for converting <see cref="string"/> to <see cref="StringWrapper"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> instance to wrap.</param>
        public static implicit operator string(StringWrapper source) => source.Value;
    }
}