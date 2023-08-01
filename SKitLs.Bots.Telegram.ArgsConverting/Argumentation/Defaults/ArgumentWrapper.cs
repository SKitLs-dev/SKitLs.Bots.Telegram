using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Defaults
{
    /// <summary>
    /// Special wrapper used for wrapping custom classes to integrate them into <see cref="IArgedAction{TArg, TUpdate}"/>
    /// as serializable argument.
    /// <para>
    /// Specific <see cref="ConvertRule{TOut}"/> for <typeparamref name="T"/> should be defined in your
    /// <see cref="IArgsSerializeService"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="T">Holding class.</typeparam>
    public class ArgumentWrapper<T> where T : class
    {
        /// <summary>
        /// Value that should be packed or unpacked.
        /// </summary>
        [BotActionArgument(0)]
        public T Value { get; set; } = null!;

        /// <summary>
        /// Creates a new instance of <see cref="ArgumentWrapper{T}"/> with default data.
        /// </summary>
        public ArgumentWrapper() { }
        /// <summary>
        /// Creates a new instance of <see cref="ArgumentWrapper{T}"/> with specific data.
        /// </summary>
        /// <param name="value">Value of the holding data.</param>
        public ArgumentWrapper(T value) => Value = value;
    }
}