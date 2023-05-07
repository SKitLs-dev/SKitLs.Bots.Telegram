namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model
{
    /// <summary>
    /// Абстрактный класс для создания инстурукций конвертации. Для создания инструкции используйте 
    /// <see cref="ConvertResult{Out}"/>
    /// </summary>
    public abstract class ConvertRule
    {
        /// <summary>
        /// Целевой тип конвертации
        /// </summary>
        public Type OutType { get; set; }

        protected ConvertRule(Type outType)
        {
            OutType = outType ?? throw new ArgumentNullException(nameof(outType));
        }
    }
}