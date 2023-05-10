using SKitLs.Bots.Telegram.Core.external.Localizations;
using SKitLs.Bots.Telegram.Core.external.Loggers;
using SKitLs.Bots.Telegram.Core.Model.Builders;

namespace SKitLs.Bots.Telegram.Core.external.LocalizedLoggers
{
    public interface ILocalizedLogger : ILogger, IOwnerCompilable
    {
        public LangKey DefaultLanguage { get; }
        public ILocalizator Localizator { get; }

        public void LLog(string mesKey, LogType type = LogType.Message, bool newLine = true, params string?[] format);
        public void LError(string mesKey, bool newLine = true, params string?[] format);
        public void LWarn(string mesKey, bool newLine = true, params string?[] format);
        public void LSuccess(string mesKey, bool newLine = true, params string?[] format);
        public void LSystem(string mesKey, bool newLine = true, params string?[] format);
    }
}