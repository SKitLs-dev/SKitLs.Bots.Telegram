using SKitLs.Bots.Telegram.BotProcesses.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model
{
    public  class ProcNewWrapper<T> : IProcessArguments
    {
        public T? Value { get; set; }

        public ProcNewWrapper() => Value = default;
        public ProcNewWrapper(T value) => Value = value;
    }
}