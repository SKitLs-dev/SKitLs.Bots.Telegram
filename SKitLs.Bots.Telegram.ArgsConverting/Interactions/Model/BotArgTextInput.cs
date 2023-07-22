using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    public class BotArgTextInput<TArg> : DefaultTextInput, IArgedAction<TArg, SignedMessageTextUpdate> where TArg : notnull, new()
    {
        public char SplitToken { get; set; }
        public BotArgedInteraction<TArg, SignedMessageTextUpdate> ArgAction { get; set; }

        public BotArgTextInput(string @base, BotArgedInteraction<TArg, SignedMessageTextUpdate> argAction, char splitToken = ';') : base(@base)
        {
            Action = MiddleAction;
            ArgAction = argAction;
            SplitToken = splitToken;
        }

        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => update.Text.Contains(SplitToken) && ActionNameBase == update.Text[..update.Text.IndexOf(SplitToken)];
        public ConvertResult<TArg> DeserializeArgs(SignedMessageTextUpdate update, IArgsSerilalizerService serilalizer)
            => serilalizer.Deserialize<TArg>(update.Text[(update.Text.IndexOf(SplitToken) + 1)..], SplitToken);
        public string SerializeArgs(TArg data, IArgsSerilalizerService serialize) => serialize.Serialize(data, SplitToken);
        public string GetSerializedData(TArg data, IArgsSerilalizerService serialize) => ActionNameBase + SerializeArgs(data, serialize);

        private async Task MiddleAction(SignedMessageTextUpdate update)
        {
            var argService = update.Owner.ResolveService<IArgsSerilalizerService>();
            var args = DeserializeArgs(update, argService).Value;
            await ArgAction.Invoke(args, update);
        }
    }
}