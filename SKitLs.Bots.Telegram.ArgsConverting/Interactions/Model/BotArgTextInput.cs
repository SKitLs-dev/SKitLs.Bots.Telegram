using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    public class BotArgTextInput<TArg> : DefaultTextInput, IArgedAction<TArg, SignedMessageTextUpdate> where TArg : new()
    {
        public char SplitToken { get; set; }

        public BotArgTextInput(string @base, BotInteraction<SignedMessageTextUpdate> action, char splitToken = ';')
            : base(@base, action)
        {
            SplitToken = splitToken;
        }

        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update)
            => ActionBase == update.Text[..update.Text.IndexOf(SplitToken)];

        public ConvertResult<TArg> DeserializeArgs(SignedMessageTextUpdate update, IArgsSerilalizerService serilalizer)
            => serilalizer.Deserialize<TArg>(update.Text[(update.Text.IndexOf(SplitToken) + 1)..], SplitToken);
    }
}