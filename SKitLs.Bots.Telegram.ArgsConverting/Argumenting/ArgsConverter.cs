namespace SKitLs.Bots.Telegram.ArgsInteraction.Argumenting
{
    /// <summary>
    /// Класс, обеспечивающий конвертацию текстовых аргументов в заданные шаблонами типы. 
    /// Обращение через <see cref="Instance"/>.
    /// <para>
    /// Правила конвертации задаются экземплярами класса <seealso cref="ConvertRule{Out}"/>.
    /// </para>
    /// </summary>
    public class ArgsConverter
    {
        private static readonly ArgsConverter _instance = new();
        /// <summary>
        /// Единая сущность конвертера
        /// </summary>
        public static ArgsConverter Instance => _instance;

        /// <summary>
        /// Правила конвертаций (<see cref="ConvertResult{Out}"/>)
        /// </summary>
        private List<ConvertRule> Rules { get; set; }

        /// <summary>
        /// Метод для получения правила конвертации по заданному типу.
        /// </summary>
        /// <typeparam name="Out">Целевой тип конвертации</typeparam>
        /// <returns>Правило конвертации строки в заданный тип.</returns>
        public ConvertRule<Out>? GetByResType<Out>()
            => (ConvertRule<Out>?)System.Convert.ChangeType(
                Rules.Find(rule => rule.OutType == typeof(Out)),
                typeof(ConvertRule<Out>), null);

        /// <summary>
        /// Метод для определения наличия правила для перевода строки в заданный тип.
        /// </summary>
        /// <typeparam name="Out">Целевой тип конвертации</typeparam>
        /// <returns><c>True</c>, если правило определено. Иначе <c>False</c>.</returns>
        public bool IsDefined<Out>() => GetByResType<Out>() != null;

        /// <summary>
        /// Конструктор для создания конвертера, автоматический добавляющий правила конвертации в типы
        /// <see cref="int"/>, <see cref="long"/>, <see cref="float"/>, <see cref="double"/>
        /// </summary>
        public ArgsConverter()
        {
            Rules = new()
            {
                new ConvertRule<int>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult.NullInput<int>();
                    else if (!int.TryParse(input, out int res))
                        return ConvertResult.Incorrect<int>();
                    else
                        return ConvertResult.OK<int>(res);
                }),
                new ConvertRule<long>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult.NullInput<long>();
                    else if (!long.TryParse(input, out long res))
                        return ConvertResult.Incorrect<long>();
                    else
                        return ConvertResult.OK<long>(res);
                }),
                new ConvertRule<float>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult.NullInput<float>();
                    else if (!float.TryParse(input, out float res))
                        return ConvertResult.Incorrect<float>();
                    else
                        return ConvertResult.OK<float>(res);
                }),
                new ConvertRule<double>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult.NullInput<double>();
                    else if (!double.TryParse(input, out double res))
                        return ConvertResult.Incorrect<double>();
                    else
                        return ConvertResult.OK<double>(res);
                }),
                new ConvertRule<bool>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult.NullInput<bool>();
                    else if (!bool.TryParse(input, out bool res))
                        return ConvertResult.Incorrect<bool>();
                    else
                        return ConvertResult.OK<bool>(res);
                }),
                new ConvertRule<string>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult.NullInput<string>();
                    else
                        return ConvertResult.OK<string>(input);
                })
            };
        }

        /// <summary>
        /// Добавляет правило конвертации в список правил. 
        /// Попытка добавления существующего правила без метки переопределения вызовет ошибку.
        /// </summary>
        /// <typeparam name="Out">Целевой тип конвертации</typeparam>
        /// <param name="rule">Правило для добавления</param>
        /// <param name="override">Метка переопределения</param>
        /// <exception cref="InvalidOperationException">Попытка добавления существующего правила без переопределения</exception>
        public void AddRule<Out>(ConvertRule<Out> rule, bool @override = false)
        {
            ConvertRule<Out>? existing = GetByResType<Out>();
            if (existing != null)
            {
                if (@override)
                {
                    Rules.Remove(existing);
                    Rules.Add(rule);
                }
                else
                    throw new InvalidOperationException();
            }
            else
                Rules.Add(rule);
        }

        /// <summary>
        /// Удаляет все текущие правила
        /// </summary>
        public void Clear() => Rules.Clear();

        /// <summary>
        /// Метод пытается преобразовать строку в заданный тип. В случае, если правило не определено, 
        /// будет возвращён результат типа <see cref="ConvertResultType.NotDefinied"/>.
        /// </summary>
        /// <typeparam name="Out">Целевой тип конвертации</typeparam>
        /// <param name="input">Входная строка</param>
        /// <returns>Результат конвертации</returns>
        public ConvertResult Convert<Out>(string input)
            => GetByResType<Out>()?.Converter(input) ?? ConvertResult.NotDefined<Out>();
    }
}
