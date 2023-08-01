namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// Represents a specified converting rule, used for converting incoming <see cref="string"/> data
    /// to custom object of a type <typeparamref name="TOut"/>.
    /// </summary>
    /// <typeparam name="TOut">Targeted result type.</typeparam>
    public sealed class ConvertRule<TOut> : ConvertRule where TOut : notnull
    {
        /// <summary>
        /// An instruction of converting input <see cref="string"/> to <typeparamref name="TOut"/>.
        /// </summary>
        public Func<string, ConvertResult<TOut>> Converter { get; private set; }

        /// <summary>
        /// Creates a new instance of a <see cref="ConvertRule"/> with a specified data.
        /// </summary>
        /// <param name="converter">An instruction of converting input <see cref="string"/> to <typeparamref name="TOut"/>.</param>
        public ConvertRule(Func<string, ConvertResult<TOut>> converter) : base(typeof(TOut))
        {
            Converter = converter;
        }
    }
}