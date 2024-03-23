namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model.Converters
{
    /// <inheritdoc/>
    public sealed class LongConverter : ConverterBase<long>
    {
        /// <inheritdoc/>
        public override ConvertResult<long> Converter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return ConvertResult<long>.NullInput();
            else if (!long.TryParse(input, out long res))
                return ConvertResult<long>.Incorrect();
            else
                return ConvertResult<long>.OK(res);
        }
    }
}