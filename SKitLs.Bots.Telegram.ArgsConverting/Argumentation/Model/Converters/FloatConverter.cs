namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model.Converters
{
    /// <inheritdoc/>
    public sealed class FloatConverter : ConverterBase<float>
    {
        /// <inheritdoc/>
        public override ConvertResult<float> Converter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return ConvertResult<float>.NullInput();
            else if (!float.TryParse(input, out float res))
                return ConvertResult<float>.Incorrect();
            else
                return ConvertResult<float>.OK(res);
        }
    }
}