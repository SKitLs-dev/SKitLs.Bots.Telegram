namespace SKitLs.Bots.Telegram.ArgsInteraction.Argumenting
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

    /// <summary>
    /// Класс-инструкция для конвертации строки в заданный тип
    /// </summary>
    /// <typeparam name="Out">Целевой тип конвертации</typeparam>
    public sealed class ConvertRule<Out> : ConvertRule
    {
        /// <summary>
        /// Инструкция конвертации заданной входной строки в целевой тип конвертации.
        /// </summary>
        public Func<string, ConvertResult> Converter { get; set; }

        /// <summary>
        /// Конструктор класса-инструкции с целевым типом конвертации и инструкцией конвертации.
        /// </summary>
        /// <param name="converter">Инструкция конвертации заданной входной строки в целевой тип конвертации</param>
        public ConvertRule(Func<string, ConvertResult> converter) : base(typeof(Out))
        {
            Converter = converter;
        }
    }
}
