namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// Represents a specific conversion rule used for converting incoming <see cref="string"/> data
    /// to a custom object of type <typeparamref name="TOut"/>.
    /// </summary>
    /// <typeparam name="TOut">The targeted result type for the conversion.</typeparam>
    public sealed class ConvertRule<TOut> : ConvertRule where TOut : notnull
    {
        /// <summary>
        /// An instruction for converting input <see cref="string"/> to <typeparamref name="TOut"/>.
        /// </summary>
        public Func<string, ConvertResult<TOut>> Converter { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ConvertRule"/> class with the specified data.
        /// </summary>
        /// <param name="converter">An instruction for converting input <see cref="string"/> to <typeparamref name="TOut"/>.</param>
        public ConvertRule(Func<string, ConvertResult<TOut>> converter) : base(typeof(TOut))
        {
            Converter = converter;
        }
    }
}