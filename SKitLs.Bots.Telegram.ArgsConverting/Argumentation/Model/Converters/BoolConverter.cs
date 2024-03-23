namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model.Converters
{
    /// <inheritdoc/>
    public sealed class BoolConverter : ConverterBase<bool>
    {
        /// <inheritdoc/>
        public override ConvertResult<bool> Converter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return ConvertResult<bool>.NullInput();
            else if (!bool.TryParse(input, out bool res))
                return ConvertResult<bool>.Incorrect();
            else
                return ConvertResult<bool>.OK(res);
        }
    }
}