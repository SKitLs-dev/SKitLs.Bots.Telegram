using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IOutputMessage
    {
        public ParseMode? ParseMode { get; set; }
        public IReplyMarkup? Markup { get; set; }
    }
}