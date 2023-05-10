using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.external.Localizations;
using SKitLs.Bots.Telegram.Core.external.Loggers;
using SKitLs.Bots.Telegram.Core.Model;

namespace SKitLs.Bots.Telegram.Core.external.LocalizedLoggers
{
    public class DefaultLocalizedLogger : DefaultConsoleLogger, ILocalizedLogger
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public LangKey DefaultLanguage { get; set; }
        public ILocalizator Localizator { get; private set; }

        public DefaultLocalizedLogger(ILocalizator localizator) => Localizator = localizator;

        public void LLog(string mesKey, LogType type = LogType.Message, bool newLine = true, params string?[] format)
        {
            Console.ForegroundColor = type switch
            {
                LogType.Message => ConsoleColor.White,
                LogType.Warning => ConsoleColor.Yellow,
                LogType.Error => ConsoleColor.Red,
                LogType.Success => ConsoleColor.Green,
                _ => ConsoleColor.Gray,
            };

            string mes = Localizator.ResolveString(DefaultLanguage, mesKey, format);
            if (newLine) Console.WriteLine(mes);
            else Console.Write($"{mes} ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void LError(string mesKey, bool newLine = true, params string?[] format)
            => Log($"[X] {Localizator.ResolveString(DefaultLanguage, mesKey, format)}", LogType.Error, newLine);
        public void LWarn(string mesKey, bool newLine = true, params string?[] format)
            => Log($"[!] {Localizator.ResolveString(DefaultLanguage, mesKey, format)}", LogType.Warning, newLine);
        public void LSuccess(string mesKey, bool newLine = true, params string?[] format)
            => Log($"[✓] {Localizator.ResolveString(DefaultLanguage, mesKey, format)}", LogType.Success, newLine);
        public void LSystem(string mesKey, bool newLine = true, params string?[] format)
            => Log($"[>] {Localizator.ResolveString(DefaultLanguage, mesKey, format)}", LogType.System, newLine);
    }
}