using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    public class DefaultTextInput : DefaultBotAction<SignedMessageTextUpdate>
    {
        public bool IgnoreCase { get; set; } = true;

        public DefaultTextInput(string @base, BotInteraction<SignedMessageTextUpdate> action) : base(@base, action) { }
        [Obsolete("Remeber to override Action property")]
        protected DefaultTextInput(string @base) : base(@base) { }

        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => IgnoreCase
            ? ActionBase.ToLower() == update.Text.ToLower()
            : ActionBase == update.Text;
    }
}