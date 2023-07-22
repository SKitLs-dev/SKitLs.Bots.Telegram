using SKitLs.Bots.Telegram.BotProcesses.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses.PartialInput
{
    public class InputProcessArgs<T> : IProcessArguments
    {
        public T Value { get; set; }
        public InputProcessArgs(T value) => Value = value;

        public static implicit operator InputProcessArgs<T>(T value) => new(value);
    }
}