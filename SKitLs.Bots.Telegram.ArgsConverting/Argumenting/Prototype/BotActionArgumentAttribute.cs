namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype
{
    /// <summary>
    /// Represents an attribute used for specifying property as one that should be put in argument parsed string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BotActionArgumentAttribute : Attribute
    {
        /// <summary>
        /// Represents a serial number of this argument.
        /// </summary>
        /// <example>
        /// For proprty with [BotActionArgument(1)] and callback data "ActionName;15;objectId"
        /// "objectId" string will be used as one to be parsed.
        /// </example>
        public int ArgIndex { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="BotActionArgumentAttribute"/> with aspecified data.
        /// </summary>
        /// <param name="argIndex">Serial number of this argument.</param>
        public BotActionArgumentAttribute(int argIndex) => ArgIndex = argIndex;
    }
}