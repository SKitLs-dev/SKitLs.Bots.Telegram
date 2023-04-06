using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgsInteraction.Interactions.Model
{
    public class BotArgTextInput<TArg> : DefaultTextInput, IArgedAction<TArg, SignedMessageTextUpdate>
    {
        public BotArgTextInput(string @base, BotInteraction<SignedMessageTextUpdate> action) : base(@base, action) { }

        public ConvertResult<TArg> DeserializeArgs(SignedMessageTextUpdate update, IArgsSerilalizerService extractor)
            => extractor.Extract<TArg>(update.Text);
    }
}