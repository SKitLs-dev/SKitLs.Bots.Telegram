namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype
{
    /// <summary>
    /// Represents an interface that provides methods for declaring packable types.
    /// A packable type can be processed more accurately by the <see cref="ArgsSerializeService"/>.
    /// </summary>
    public interface IArgPackable
    {
        /// <summary>
        /// Serializes the object's data into a string representation that can be used for packing.
        /// </summary>
        /// <returns>A serialized string representation of the object's data.</returns>
        public string GetPacked();
    }
}