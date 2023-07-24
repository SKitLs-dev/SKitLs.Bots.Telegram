using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Defaults
{
    /// <summary>
    /// Special wrapper used for wrapping <see cref="string"/> data to integrate it into
    /// <see cref="IArgedAction{TArg, TUpdate}"/> as serializable argument.
    /// <para>
    /// Specified solution for an <see cref="ArgumentWrapper{T}"/>.
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

        public static implicit operator StringWrapper(string value) => new(value);
        public static implicit operator string(StringWrapper source) => source.Value;
    }
}