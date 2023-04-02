using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IOutputMessage
    {
        public ParseMode? ParseMode { get; set; }
        public IReplyMarkup? Markup { get; set; }

        public bool IsParseSafe(ParseMode mode, string part);
        public string MakeParseSafe(ParseMode mode, string part);
    }
}