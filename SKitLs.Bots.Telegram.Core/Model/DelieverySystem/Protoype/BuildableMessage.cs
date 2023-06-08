namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype
{
    /// <summary>
    /// Default realization of <see cref="IBuildableMessage"/>. Provides simple 
    /// </summary>
    public class BuildableMessage : IBuildableMessage
    {
        public string Text { get; set; }
        
        /// <summary>
        /// Creates a new instance of <see cref="BuildableMessage"/> with a specific text.
        /// </summary>
        /// <param name="text">Text to be saved</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BuildableMessage(string text) => Text = string.IsNullOrEmpty(text)
            ? throw new ArgumentNullException(nameof(text))
            : text;

        public string GetMessageText() => Text;
        public object Clone() => new BuildableMessage((string)Text.Clone());
        
        public static explicit operator BuildableMessage(string text) => new(text);
        public override string? ToString() => $"{GetType().Name} \"{Text}\"";
    }
}