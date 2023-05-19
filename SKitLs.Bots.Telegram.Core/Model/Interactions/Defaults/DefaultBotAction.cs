using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults
{
    public abstract class DefaultBotAction<TUpdate> : IBotAction<TUpdate> where TUpdate : ICastedUpdate
    {
        public virtual string ActionBase { get; private set; }
        public BotInteraction<TUpdate> Action { get; protected set; }

        public DefaultBotAction(string @base, BotInteraction<TUpdate> action)
        {
            ActionBase = @base ?? throw new ArgumentNullException(nameof(@base));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        [Obsolete("Remeber to override Action property")]
        protected DefaultBotAction(string @base)
        {
            ActionBase = @base ?? throw new ArgumentNullException(nameof(@base));
            Action = null!;
        }

        public abstract bool ShouldBeExecutedOn(TUpdate update);

        public bool Equals(IBotAction<TUpdate>? other)
        {
            if (other is null) return false;

            Type genericArg = other.GetType().GetGenericArguments()[0];
            if (genericArg.IsEquivalentTo(GetType().GetGenericArguments()[0]))
                return ActionBase == other.ActionBase;
            return false;
        }
    }
}