namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public class DefaultBotUser : IBotUser
    {
        public long Id => TelegramId;
        public long TelegramId { get; set; }
        public string DisplayName { get; set; }

        public DefaultBotUser(long id)
        {
            TelegramId = id;
            DisplayName = "name";
        }

        public string ShortDisplay() => Id.ToString();
        public string FullDisplay(int fullness) => ShortDisplay();
        public string ListDisplay() => $"Неопознанный - {Id}";
    }
}