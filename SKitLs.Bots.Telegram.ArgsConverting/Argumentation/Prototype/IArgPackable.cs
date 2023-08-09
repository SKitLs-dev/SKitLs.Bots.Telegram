namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype
{
    /// <summary>
    /// Represents an interface that provides methods for declaring packable types.
    /// A packable type can be processed more accurately by the <see cref="DefaultArgsSerializeService"/>.
    /// </summary>
    public interface IArgPackable
    {
        /// <summary>
        /// Generates a string that can be used as a representation of this object.
        /// </summary>
        /// <returns>A string representing this object's data.</returns>
        public string GetPacked();
    }
}