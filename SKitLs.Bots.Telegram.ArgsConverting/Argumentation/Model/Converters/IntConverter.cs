namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model.Converters
{
    /// <inheritdoc/>
    public sealed class IntConverter : ConverterBase<int>
    {
        /// <inheritdoc/>
        public override ConvertResult<int> Converter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return ConvertResult<int>.NullInput();
            else if (!int.TryParse(input, out int res))
                return ConvertResult<int>.Incorrect();
            else
                return ConvertResult<int>.OK(res);
        }
    }
}
