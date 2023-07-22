using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Extensions
{
    public class ArgWrap<T> where T : class
    {
        [BotActionArgument(0)]
        public T Value { get; set; } = null!;

        public ArgWrap() { }
        public ArgWrap(T value) => Value = value;
    }
}