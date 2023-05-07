using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    public class BotArgedCallback<TArg> : DefaultCallback, IArgedAction<TArg, SignedCallbackUpdate> where TArg : new()
    {
        public char SplitToken { get; set; }

        public BotArgedCallback(string @base, string label, BotInteraction<SignedCallbackUpdate> action, char splitToken = ';')
            : base(@base, label, action)
        {
            SplitToken = splitToken;
        }

        public override bool ShouldBeExecutedOn(SignedCallbackUpdate update)
            => ActionBase == update.Data[..update.Data.IndexOf(SplitToken)];

        public ConvertResult<TArg> DeserializeArgs(SignedCallbackUpdate update, IArgsSerilalizerService serilalizer)
            => serilalizer.Deserialize<TArg>(update.Data[(update.Data.IndexOf(SplitToken) + 1)..], SplitToken);
    }
}