using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    public abstract class OutputMessage : IOutputMessage
    {
        public ParseMode? ParseMode { get; set; }
        public IReplyMarkup? Markup { get; set; }

        public OutputMessage UseParseMode(ParseMode mode)
        {
            ParseMode = mode;
            return this;
        }
        public OutputMessage AddMarkup(IReplyMarkup? markup)
        {
            Markup = markup;
            return this;
        }

        public string GetMessageText()
        {
            throw new NotImplementedException();
        }
    }
}
