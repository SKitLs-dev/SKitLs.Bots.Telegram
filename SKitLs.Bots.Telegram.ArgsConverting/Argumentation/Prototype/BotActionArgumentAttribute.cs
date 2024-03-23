namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype
{
    /// <summary>
    /// Represents an attribute used to specify a property as one that should be included in the parsed argument string for a bot action.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BotActionArgumentAttribute"/> class with the specified argument index.
    /// </remarks>
    /// <param name="argIndex">The serial number of this argument.</param>
    [AttributeUsage(AttributeTargets.Property)]
    public class BotActionArgumentAttribute(int argIndex) : Attribute
    {
        /// <summary>
        /// Gets the serial number of this argument.
        /// </summary>
        /// <remarks>
        /// For a property with <c>[BotActionArgument(1)]</c> and callback data <i>"ActionName;15;objectId"</i>,
        /// the string "objectId" will be used as the argument to be parsed.
        /// </remarks>
        public int ArgIndex { get; private init; } = argIndex;
    }
}