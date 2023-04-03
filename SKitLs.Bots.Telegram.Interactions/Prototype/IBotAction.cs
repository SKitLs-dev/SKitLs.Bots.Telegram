using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Interactions.Prototype
{
    public delegate Task BotInteraction<TUpdate>(IBotAction<TUpdate> trigger, TUpdate update) where TUpdate : ICastedUpdate;

    public interface IBotAction<TUpdate> : IEquatable<IBotAction<ICastedUpdate>> where TUpdate : ICastedUpdate
    {
        public string ActionBase { get; }
        public BotInteraction<TUpdate> Action { get; }

        public bool IsSimilarWith(IBotAction<TUpdate> action);
        public bool ShouldBeExecutedOn(TUpdate update);
    }
}