using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions
{
    public delegate Task BotArgedInteraction<TArg, TUpdate>(IArgedAction<TArg, TUpdate> trigger, TArg args, TUpdate update) where TUpdate : ICastedUpdate;

    public interface IArgedAction<TArg, TUpdate> : IBotAction<TUpdate> where TUpdate : ICastedUpdate
    {
        public char SplitToken { get; set; }
        public BotArgedInteraction<TArg, TUpdate> ArgAction { get; }
        public ConvertResult<TArg> DeserializeArgs(TUpdate update, IArgsSerilalizerService serializer);
        public string SerializeArgs(TArg data, IArgsSerilalizerService serialize);
        public string GetSerializedData(TArg data, IArgsSerilalizerService serialize);
    }
}