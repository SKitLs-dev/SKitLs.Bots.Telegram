using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    public class DefaultCallback : IBotAction<SignedCallbackUpdate>
    {
        public string Label { get; private set; }
        public string ActionBase { get; private set; }
        public BotInteraction<SignedCallbackUpdate> Action { get; private set; }

        public DefaultCallback(string @base, string label, BotInteraction<SignedCallbackUpdate> action)
        {
            ActionBase = @base;
            Action = action;
            Label = label;
        }

        public virtual bool ShouldBeExecutedOn(SignedCallbackUpdate update)
            => ActionBase == update.Data;

        public bool Equals(IBotAction<ICastedUpdate>? other)
        {
            if (other is null) return false;

            Type genericArg = other.GetType().GetGenericArguments()[0];
            if (genericArg.IsEquivalentTo(GetType()))
            {

            }
            return false;
        }
    }
}