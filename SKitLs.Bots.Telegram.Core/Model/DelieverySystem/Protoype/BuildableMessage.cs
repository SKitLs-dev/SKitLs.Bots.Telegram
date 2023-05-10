namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype
{
    public class BuildableMessage : IBuildableMessage
    {
        public string Text { get; set; }
        public BuildableMessage(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            Text = text;
        }
        public string GetMessageText() => Text;
        public object Clone() => new BuildableMessage(Text);
        
        public static explicit operator BuildableMessage(string text) => new(text);
        public override string? ToString() => $"{GetType().Name} \"{Text}\"";
    }
}