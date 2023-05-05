using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    public class DefaultTextInput : IBotAction<SignedMessageTextUpdate>
    {
        //public int ExecutionWeight { get; set; }
        //public SignedTextUpdatePredicate PredicateExecution { get; private set; }
        //public SignedTextUpdate Executer { get; set; }

        public string ActionBase { get; private set; }
        public BotInteraction<SignedMessageTextUpdate> Action { get; private set; }

        public DefaultTextInput(string @base, BotInteraction<SignedMessageTextUpdate> action)
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
