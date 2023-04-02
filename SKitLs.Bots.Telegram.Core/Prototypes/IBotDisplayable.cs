namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IBotDisplayable
    {
        public long Id { get; }

        public string DisplayName { get; }
        public string ShortDisplay();
        public string FullDisplay(int fullness = 0);
        public string ListDisplay();
    }
}