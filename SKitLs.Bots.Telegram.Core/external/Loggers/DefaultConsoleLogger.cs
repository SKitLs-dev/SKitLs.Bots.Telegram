namespace SKitLs.Bots.Telegram.Core.external.Loggers
{
    public class DefaultConsoleLogger : ILogger
    {
        public void Log(string message, LogType type = LogType.Message, bool newLine = true)
        {
            Console.ForegroundColor = type switch
            {
                LogType.Message => ConsoleColor.White,
                LogType.Warning => ConsoleColor.Yellow,
                LogType.Error => ConsoleColor.Red,
                LogType.Success => ConsoleColor.Green,
                _ => ConsoleColor.Gray,
            };
            if (newLine) Console.WriteLine(message);
            else Console.Write($"{message} ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Error(string message, bool newLine = true) => Log($"[X] {message}", LogType.Error, newLine);
        public void Warn(string message, bool newLine = true) => Log($"[!] {message}", LogType.Warning, newLine);
        public void Success(string message, bool newLine = true) => Log($"[✓] {message}", LogType.Success, newLine);
        public void System(string messgae, bool newLine = true) => Log($"[>] {messgae}", LogType.System, newLine);
        public void Line() => Log("");
    }
}
