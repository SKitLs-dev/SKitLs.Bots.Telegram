namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    /// <summary>
    /// Represents a converting status.
    /// </summary>
    public enum ConvertResultType
    {
        /// <summary>
        /// Represents a succeed converting.
        /// </summary>
        Ok,

        /// <summary>
        /// Represents null input status (for <see cref="string.IsNullOrEmpty(string?)"/> input).
        /// </summary>
        NullInput,

        /// <summary>
        /// Represents incorrect input status.
        /// </summary>
        Incorrect,

        /// <summary>
        /// Represents not presented status for cases when converting is being collected from some collection.
        /// </summary>
        NotPresented,

        /// <summary>
        /// Represents not defined status for cases when appropriate <see cref="ConvertRule{TOut}"/>
        /// was not presented in <see cref="IArgsSerializeService"/>.
        /// </summary>
        NotDefined
    }
}