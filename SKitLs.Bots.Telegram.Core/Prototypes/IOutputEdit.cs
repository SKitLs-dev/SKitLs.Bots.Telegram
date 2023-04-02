using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IOutputEdit
    {
        public int EditMessageId { get; set; }
        public InlineKeyboardMarkup? InlineMarkup { get; }
    }
}
