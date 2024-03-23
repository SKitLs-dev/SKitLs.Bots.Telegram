namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model.Converters
{
    /// <inheritdoc/>
    public sealed class DoubleConverter : ConverterBase<double>
    {
        /// <inheritdoc/>
        public override ConvertResult<double> Converter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return ConvertResult<double>.NullInput();
            else if (!double.TryParse(input, out double res))
                return ConvertResult<double>.Incorrect();
            else
                return ConvertResult<double>.OK(res);
        }
    }
}