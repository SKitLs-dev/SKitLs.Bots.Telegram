using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting
{
    public interface IArgsSerilalizerService
    {
        public string Serialize<TIn>(TIn input, char splitToken);
        public ConvertResult<TOut> Deserialize<TOut>(string input, char splitToken) where TOut : new();

        public string Pack<TIn>(TIn input);
        public ConvertResult<TOut> Unpack<TOut>(string input);
    }
}