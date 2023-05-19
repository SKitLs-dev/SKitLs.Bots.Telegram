using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    public delegate Task BotInteraction<TUpdate>(IBotAction<TUpdate> trigger, TUpdate update) where TUpdate : ICastedUpdate;

    public interface IBotAction<TUpdate> : IEquatable<IBotAction<TUpdate>> where TUpdate : ICastedUpdate
    {
        public string ActionBase { get; }
        public BotInteraction<TUpdate> Action { get; }

        public bool ShouldBeExecutedOn(TUpdate update);
    }
}