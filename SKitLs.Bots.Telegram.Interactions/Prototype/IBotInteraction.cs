namespace SKitLs.Bots.Telegram.Interactions.Prototype
{
    public interface IBotInteraction
    {
        public string Base { get; }
        public bool IsSimilarWith(IBotInteraction interaction);
        public bool ShouldBeExecutedOn(string message);
    }
}
