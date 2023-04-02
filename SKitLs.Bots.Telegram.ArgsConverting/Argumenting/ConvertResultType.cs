namespace SKitLs.Bots.Telegram.ArgsConverting.Argumenting
{
    /// <summary>
    /// Тип результата конвертации.
    /// </summary>
    public enum ConvertResultType
    {
        /// <summary>
        /// Успешная конвертация
        /// </summary>
        Ok,
        /// <summary>
        /// Входные данные отсутствовали (<see cref="string.IsNullOrEmpty(string?)"/>)
        /// </summary>
        NullInput,
        /// <summary>
        /// Входные данные частично успешно конвертированы. См. <see cref="ConvertResultList"/>
        /// </summary>
        Semicorrect,
        /// <summary>
        /// Входные данные имели неверный формат
        /// </summary>
        Incorrect,
        /// <summary>
        /// Данные входные данные не представлены в метаданных
        /// </summary>
        NotPresented,
        /// <summary>
        /// Правила сущности конвертера <see cref="ArgsConverter.Rules"/> не содержали определений для конвертации 
        /// входной строки в заданный тип.
        /// </summary>
        NotDefinied
    }
}
