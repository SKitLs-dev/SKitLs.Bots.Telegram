namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype
{
    /// <summary>
    /// Represents an interface that provides methods of declaring packable types.
    /// Packable type can be processed by <see cref="DefaultArgsSerilalizerService"/> more correctly.
    /// </summary>
    public interface IArgPackable
    {
        /// <summary>
        /// Generates a string that could be used as a representation of this object.
        /// </summary>
        /// <returns>A string that could be used as a representation of this object.</returns>
        public string GetPacked();
    }
}