using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting
{
    public interface IArgsSerilalizerService
    {
        public ConvertResult<TOut> Extract<TOut>(string input);
    }
}