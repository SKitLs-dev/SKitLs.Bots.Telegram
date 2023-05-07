using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions
{
    public interface IArgedAction<TArg, TUpdate> : IBotAction<TUpdate> where TUpdate : ICastedUpdate
    {
        public char SplitToken { get; set; }

        public ConvertResult<TArg> DeserializeArgs(TUpdate update, IArgsSerilalizerService extractor);
    }
}