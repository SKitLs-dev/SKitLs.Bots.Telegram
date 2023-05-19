using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    public class DefaultCallback : DefaultBotAction<SignedCallbackUpdate>
    {
        public string Label { get; private set; }

        public DefaultCallback(string @base, string label, BotInteraction<SignedCallbackUpdate> action)
            : base(@base, action) => Label = label;

        [Obsolete("Remeber to override Action property")]
        protected DefaultCallback(string @base, string label) : base(@base) => Label = label;

        public override bool ShouldBeExecutedOn(SignedCallbackUpdate update) => ActionBase == update.Data;
    }
}