namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype
{
    /// <summary>
    /// Default realization of <see cref="IBuildableMessage"/>.
    /// </summary>
    public class BuildableMessage : IBuildableMessage
    {
        /// <summary>
        /// Text to be sent.
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Creates a new instance of <see cref="BuildableMessage"/> with a specific text.
        /// </summary>
        /// <param name="text">Text to be saved.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BuildableMessage(string text) => Text = string.IsNullOrEmpty(text)
            ? throw new ArgumentNullException(nameof(text))
            : text;

        /// <summary>
        /// Builds object's data and packs it into one text so it could be easily sent to server.
        /// </summary>
        /// <returns>Valid text, ready to be sent.</returns>
        [Obsolete("Will be replaced with IDynamic methods from *.AdvancedMessages")]
        public string GetMessageText() => Text;

        /// <summary>
        /// Creates a new object that is copy of the current instance.
        /// </summary>
        /// <returns>A new object that is copy of the current instance.</returns>
        public object Clone() => new BuildableMessage((string)Text.Clone());

        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override string? ToString() => $"{GetType().Name} \"{Text}\"";
    }
}