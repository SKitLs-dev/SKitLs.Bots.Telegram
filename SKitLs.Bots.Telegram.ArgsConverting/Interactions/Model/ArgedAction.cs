using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    public class ArgedAction<TArg, TUpdate> : IArgedAction<TArg, TUpdate> where TUpdate : ICastedUpdate
    {
        public string ActionBase { get; private set; }
        public BotInteraction<TUpdate> Action { get; private set; }

        public ArgedAction(string actionBase, BotInteraction<TUpdate> action)
        {
            ActionBase = actionBase;
            Action = action;
        }

        public bool Equals(IBotAction<ICastedUpdate>? other)
        {
            throw new NotImplementedException();
        }

        public ConvertResult<TArg> DeserializeArgs(TUpdate update, IArgsSerilalizerService extractor)
        {
            throw new NotImplementedException();
        }

        public bool ShouldBeExecutedOn(TUpdate update)
        {
            throw new NotImplementedException();
        }
    }
}
