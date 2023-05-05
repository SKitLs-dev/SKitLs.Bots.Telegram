using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    public class DefaultCommand : IBotAction<SignedMessageTextUpdate>
    {
        public string ActionBase { get; private set; }
        public BotInteraction<SignedMessageTextUpdate> Action { get; private set; }

        public DefaultCommand(string @base, BotInteraction<SignedMessageTextUpdate> action)
        {
            ActionBase = @base;
            Action = action;
        }

        public virtual bool ShouldBeExecutedOn(SignedMessageTextUpdate update)
            => ActionBase == update.Text;

        public bool Equals(IBotAction<ICastedUpdate>? other)
        {
            throw new NotImplementedException();
        }
    }
}