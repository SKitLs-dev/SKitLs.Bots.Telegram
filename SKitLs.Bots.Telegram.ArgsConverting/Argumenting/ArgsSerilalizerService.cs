using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;

namespace SKitLs.Bots.Telegram.ArgsInteraction.Argumenting
{
    /// <summary>
    /// Класс, обеспечивающий конвертацию текстовых аргументов в заданные шаблонами типы. 
    /// Обращение через <see cref="Instance"/>.
    /// <para>
    /// Правила конвертации задаются экземплярами класса <seealso cref="ConvertRule{Out}"/>.
    /// </para>
    /// </summary>
    public class ArgsSerilalizerService : IArgsSerilalizerService
    {
        /// <summary>
        /// Правила конвертаций (<see cref="ConvertResult{TOut}"/>)
        /// </summary>
        private List<ConvertRule> Rules { get; set; }

        /// <summary>
        /// Метод для получения правила конвертации по заданному типу.
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <returns>Правило конвертации строки в заданный тип.</returns>
        public ConvertRule<TOut>? GetByResType<TOut>()
            => (ConvertRule<TOut>?)System.Convert.ChangeType(
                Rules.Find(rule => rule.OutType == typeof(TOut)),
                typeof(ConvertRule<TOut>), null);

        /// <summary>
        /// Метод для определения наличия правила для перевода строки в заданный тип.
        /// </summary>
        /// <typeparam name="Out">Целевой тип конвертации</typeparam>
        /// <returns><c>True</c>, если правило определено. Иначе <c>False</c>.</returns>
        public bool IsDefined<TOut>() => GetByResType<TOut>() != null;

        /// <summary>
        /// Конструктор для создания конвертера, автоматический добавляющий правила конвертации в типы
        /// <see cref="int"/>, <see cref="long"/>, <see cref="float"/>, <see cref="double"/>
        /// </summary>
        public ArgsSerilalizerService()
        {
            Rules = new()
            {
                new ConvertRule<int>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<int>.NullInput();
                    else if (!int.TryParse(input, out int res))
                        return ConvertResult<int>.Incorrect();
                    else
                        return ConvertResult<int>.OK(res);
                }),
                new ConvertRule<long>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<long>.NullInput();
                    else if (!long.TryParse(input, out long res))
                        return ConvertResult<long>.Incorrect();
                    else
                        return ConvertResult<long>.OK(res);
                }),
                new ConvertRule<float>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<float>.NullInput();
                    else if (!float.TryParse(input, out float res))
                        return ConvertResult<float>.Incorrect();
                    else
                        return ConvertResult<float>.OK(res);
                }),
                new ConvertRule<double>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<double>.NullInput();
                    else if (!double.TryParse(input, out double res))
                        return ConvertResult<double>.Incorrect();
                    else
                        return ConvertResult<double>.OK(res);
                }),
                new ConvertRule<bool>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<bool>.NullInput();
                    else if (!bool.TryParse(input, out bool res))
                        return ConvertResult<bool>.Incorrect();
                    else
                        return ConvertResult<bool>.OK(res);
                }),
                new ConvertRule<string>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<string>.NullInput();
                    else
                        return ConvertResult<string>.OK(input);
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
        public void AddRule<TOut>(ConvertRule<TOut> rule, bool @override = false)
        {
            ConvertRule<TOut>? existing = GetByResType<TOut>();
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
        public ConvertResult<TOut> Extract<TOut>(string input)
            => GetByResType<TOut>()?.Converter(input) ?? ConvertResult<TOut>.NotDefined();
    }
}