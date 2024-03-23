namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// Represents a base class for converters used to convert input strings to a specific type.
    /// </summary>
    /// <typeparam name="TOut">The type to which the input string will be converted.</typeparam>
    public abstract class ConverterBase<TOut> : ConvertRule where TOut : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterBase{TOut}"/> class.
        /// </summary>
        protected ConverterBase() : base(typeof(TOut)) { }

        /// <inheritdoc/>
        public override ConvertResult Convert(string input) => Converter(input);

        /// <summary>
        /// Converts the input string to the specified output type.
        /// </summary>
        /// <param name="input">The input string to be converted.</param>
        /// <returns>A <see cref="ConvertResult{TOut}"/> containing the result of the conversion.</returns>
        public abstract ConvertResult<TOut> Converter(string input);
    }
}