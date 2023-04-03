using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Interactions.Prototype;
using System.Runtime.CompilerServices;

namespace SKitLs.Bots.Telegram.Interactions.Model
{
    public class BotCallback : IBotAction<SignedCallbackUpdate>
    {
        public string Label { get; private set; }
        public string ActionBase { get; private set; }
        public BotInteraction<SignedCallbackUpdate> Action { get; private set; }

        public BotCallback(string @base, string label, BotInteraction<SignedCallbackUpdate> action)
        {
            ActionBase = @base;
            Action = action;
            Label = label;
        }

        public BotCallback ExtendLabel(string label)
        {
            Label += " " + label;
            return this;
        }

        public bool IsSimilarWith(IBotAction<SignedCallbackUpdate> action)
        {
            throw new NotImplementedException();
        }

        public bool ShouldBeExecutedOn(SignedCallbackUpdate update)
        {
            throw new NotImplementedException();
        }

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