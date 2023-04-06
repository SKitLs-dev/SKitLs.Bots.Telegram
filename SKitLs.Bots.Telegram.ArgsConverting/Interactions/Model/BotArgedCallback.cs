using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgsInteraction.Interactions.Model
{
    public class BotArgedCallback<TArg> : DefaultCallback, IArgedAction<TArg, SignedCallbackUpdate>
    {
        public BotArgedCallback(string @base, string label, BotInteraction<SignedCallbackUpdate> action) : base(@base, label, action) { }

        public ConvertResult<TArg> DeserializeArgs(SignedCallbackUpdate update, IArgsSerilalizerService extractor)
            => extractor.Extract<TArg>(update.Data);
    }
}