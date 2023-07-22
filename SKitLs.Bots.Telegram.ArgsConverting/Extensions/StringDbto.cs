using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Extensions
{
    public class StringDbto
    {
        [BotActionArgument(0)]
        public string Content { get; set; }

        public StringDbto() => Content = string.Empty;
        public StringDbto(string content) => Content = content;

        public static implicit operator StringDbto(string content) => new(content);
        public static implicit operator string(StringDbto source) => source.Content;
    }
}