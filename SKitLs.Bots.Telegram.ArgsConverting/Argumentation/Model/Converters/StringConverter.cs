namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model.Converters
{
    /// <inheritdoc/>
    public sealed class StringConverter : ConverterBase<string>
    {
        /// <inheritdoc/>
        public override ConvertResult<string> Converter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return ConvertResult<string>.NullInput();
            else
                return ConvertResult<string>.OK(input);
        }
    }
}