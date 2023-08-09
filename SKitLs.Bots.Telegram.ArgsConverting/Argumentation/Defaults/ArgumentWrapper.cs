using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Defaults
{
    /// <summary>
    /// Special wrapper designed for encapsulating custom classes with no public parameterless constructors
    /// to integrate them into <see cref="IArgedAction{TArg, TUpdate}"/> as serializable arguments.
    /// <para>
    /// A specific <see cref="ConvertRule{TOut}"/> for <typeparamref name="T"/> should be defined in your
    /// <see cref="IArgsSerializeService"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the encapsulated class.</typeparam>
    public class ArgumentWrapper<T> where T : notnull
    {
        /// <summary>
        /// The value that should be packed or unpacked.
        /// </summary>
        [BotActionArgument(0)]
        public T Value { get; set; } = default!;

        /// <summary>
        /// Creates a new instance of the <see cref="ArgumentWrapper{T}"/> class with default data.
        /// </summary>
        public ArgumentWrapper() { }
        /// <summary>
        /// Creates a new instance of the <see cref="ArgumentWrapper{T}"/> class with specific data.
        /// </summary>
        /// <param name="value">The value of the encapsulated data.</param>
        public ArgumentWrapper(T value) => Value = value;
    }
}