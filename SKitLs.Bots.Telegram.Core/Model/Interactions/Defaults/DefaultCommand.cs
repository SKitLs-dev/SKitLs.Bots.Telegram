using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    public class DefaultCommand : DefaultBotAction<SignedMessageTextUpdate>
    {
        public DefaultCommand(string @base, BotInteraction<SignedMessageTextUpdate> action) : base(@base, action) { }

        [Obsolete("Remeber to override Action property")]
        protected DefaultCommand(string @base) : base(@base) { }

        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => $"/{ActionBase}" == update.Text;
    }
}