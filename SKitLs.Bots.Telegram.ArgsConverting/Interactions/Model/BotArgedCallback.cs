using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    public class BotArgedCallback<TArg> : DefaultCallback, IArgedAction<TArg, SignedCallbackUpdate> where TArg : notnull, new()
    {
        public char SplitToken { get; set; }

        public BotArgedInteraction<TArg, SignedCallbackUpdate> ArgAction { get; private set; }

        public BotArgedCallback(string @base, string label, BotArgedInteraction<TArg, SignedCallbackUpdate> argAction, char splitToken = ';') : base(@base, label)
        {
            Action = MiddleAction;
            ArgAction = argAction;
            SplitToken = splitToken;
        }

        public override bool ShouldBeExecutedOn(SignedCallbackUpdate update) => ActionBase == update.Data[..update.Data.IndexOf(SplitToken)];
        public ConvertResult<TArg> DeserializeArgs(SignedCallbackUpdate update, IArgsSerilalizerService serilalizer)
            => serilalizer.Deserialize<TArg>(update.Data[(update.Data.IndexOf(SplitToken) + 1)..], SplitToken);
        public string SerializeArgs(TArg data, IArgsSerilalizerService serialize) => serialize.Serialize(data, SplitToken);
        public string GetSerializedData(TArg data, IArgsSerilalizerService serialize) => ActionBase + SerializeArgs(data, serialize);

        private async Task MiddleAction(IBotAction<SignedCallbackUpdate> trigger, SignedCallbackUpdate update)
        {
            var argedAction = trigger as IArgedAction<TArg, SignedCallbackUpdate> ?? throw new Exception();
            var argService = update.Owner.ResolveService<IArgsSerilalizerService>();
            var args = DeserializeArgs(update, argService).Value;
            await ArgAction.Invoke(argedAction, args, update);
        }
    }
}