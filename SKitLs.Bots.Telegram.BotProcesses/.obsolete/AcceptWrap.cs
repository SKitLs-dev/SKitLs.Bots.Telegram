namespace SKitLs.Bots.Telegram.BotProcesses.obsolete
{
    public class AcceptationWrapper<T>
    {
        public T Value { get; set; }
        public bool Result { get; set; }
        public AcceptationWrapper(T value) => Value = value;
    }
}