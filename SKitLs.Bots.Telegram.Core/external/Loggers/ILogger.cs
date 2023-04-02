namespace SKitLs.Bots.Telegram.Core.external.Loggers
{
    public interface ILogger
    {
        public void Log(string message, LogType type = LogType.Message, bool newLine = true);

        public void Error(string message, bool newLine = true);
        public void Warn(string message, bool newLine = true);
        public void Success(string message, bool newLine = true);
        public void System(string message, bool newLine = true);
        public void Line();
    }
}