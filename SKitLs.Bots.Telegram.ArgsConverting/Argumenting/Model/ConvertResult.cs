namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model
{
    /// <summary>
    /// Класс-оболочка для результатов конвертации
    /// </summary>
    /// <typeparam name="Out">Целевой тип конвертации</typeparam>
    public class ConvertResult<TOut>
    {
        /// <summary>
        /// Значение результата конвертации.
        /// </summary>
        public TOut? Value { get; set; }
        public Type ValueType { get; set; }
        /// <summary>
        /// Тип результата конвертации.
        /// </summary>
        public ConvertResultType ResultType { get; set; }
        /// <summary>
        /// Сообщение результата конвертации.
        /// </summary>
        public string Message { get; set; }

        public ConvertResult(Type valueType, string message)
        {
            ValueType = valueType;
            Message = message;
        }

        /// <summary>
        /// Создаёт успешный (<see cref="ConvertResultType.Ok"/>) результат конвертации с заданным 
        /// выходным значением.
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <param name="value">Выходное значение</param>
        /// <returns>Успешный результат конвертации.</returns>
        public static ConvertResult<TOut> OK(TOut value)
            => new(typeof(TOut), "OK")
            {
                Value = value,
                ResultType = ConvertResultType.Ok,
            };

        /// <summary>
        /// Создаёт успешный (<see cref="ConvertResultType.Ok"/>) результат конвертации с заданным 
        /// выходным значением и пользовательским сообщением результата конвертации.
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <param name="value">Выходное значение</param>
        /// <param name="caption">Пользовательское сообщение результата конвертации</param>
        /// <returns>Успешный результат конвертации.</returns>
        public static ConvertResult<TOut> OK(TOut value, string caption)
            => new(typeof(TOut), caption)
            {
                Value = value,
                ResultType = ConvertResultType.Ok,
            };

        /// <summary>
        /// Создаёт результат конвертации с ошибкой типа <see cref="ConvertResultType.NullInput"/> в случае 
        /// нулевого или пустого входного значения.
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <returns>Результат конвертации с ошибкой.</returns>
        public static ConvertResult<TOut> NullInput()
            => new(typeof(TOut), "Введённые данные отсутствовали")
            {
                Value = default,
                ResultType = ConvertResultType.NullInput,
            };

        /// <summary>
        /// Создаёт результат конвертации с ошибкой типа <see cref="ConvertResultType.Incorrect"/> и сообщением 
        /// по умолчанию в случае ошибки преобразования конвертера.
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <returns>Результат конвертации с ошибкой.</returns>
        public static ConvertResult<TOut> Incorrect()
            => new(typeof(TOut), "Введённые данные имели неверный формат")
            {
                Value = default,
                ResultType = ConvertResultType.Incorrect,
            };

        /// <summary>
        /// Создаёт результат конвертации с ошибкой типа <see cref="ConvertResultType.Incorrect"/> и заданным 
        /// сообщением в случае ошибки преобразования конвертера.
        /// <para>
        /// Для добавления описания к сообщению по умолчанию оставьте маркер перезаписи False. Для замещения 
        /// слияния строк на только заданное описание включите маркер перезаписи.
        /// </para>
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <param name="definition">Описание ошибки</param>
        /// <param name="override">Маркер перезаписи</param>
        /// <returns>Результат конвертации с ошибкой.</returns>
        public static ConvertResult<TOut> Incorrect(string definition, bool @override = false)
            => new(typeof(TOut), @override ? definition : "Введённые данные имели неверный формат: " + definition)
            {
                Value = default,
                ResultType = ConvertResultType.Incorrect
            };

        /// <summary>
        /// Создаёт результат конвертации с ошибкой типа <see cref="ConvertResultType.NotPresented"/> и сообщением 
        /// по умолчанию в случае ошибки преобразования конвертера.
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <returns>Результат конвертации с ошибкой.</returns>
        public static ConvertResult<TOut> NotPresented()
            => new(typeof(TOut), "Входные данные не были представлены в метаданных")
            {
                Value = default,
                ResultType = ConvertResultType.NotPresented
            };

        /// <summary>
        /// Создаёт результат конвертации с ошибкой типа <see cref="ConvertResultType.NotPresented"/> и заданным 
        /// сообщением в случае ошибки преобразования конвертера.
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <param name="definition">Описание ошибки</param>
        /// <returns>Результат конвертации с ошибкой.</returns>
        public static ConvertResult<TOut> NotPresented(string definition)
            => new(typeof(TOut), definition)
            {
                Value = default,
                ResultType = ConvertResultType.NotPresented,
            };

        /// <summary>
        /// Создаёт результат конвертации с ошибкой типа <see cref="ConvertResultType.NotDefinied"/> в случае, 
        /// если список правил <see cref="DefaultArgsSerilalizerService.Rules"/> не содержит определения конвертации входной строки 
        /// в заданный тип.
        /// </summary>
        /// <typeparam name="TOut">Целевой тип конвертации</typeparam>
        /// <returns>Результат конвертации с ошибкой неопределённости.</returns>
        internal static ConvertResult<TOut> NotDefined()
            => new(typeof(TOut), $"Converter rule for type {typeof(TOut)} is not defined")
            {
                Value = default,
                ResultType = ConvertResultType.NotDefinied,
            };
    }
}
