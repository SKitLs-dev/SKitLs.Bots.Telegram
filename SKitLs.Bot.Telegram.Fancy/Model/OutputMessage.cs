using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TEnums = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Fancy.Model
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
        public bool IsParseSafe(ParseMode mode, string part) => mode switch
        {
            TEnums.ParseMode.Markdown => IsMarkdownSafe(part),
            _ => true,
        };
        public string MakeParseSafe(ParseMode mode, string part) => mode switch
        {
            TEnums.ParseMode.Markdown => MakeMarkdownSafe(part),
            _ => part,
        };

        private bool IsMarkdownSafe(string part)
        {
            int italic = 0;
            int bold = 0;
            int stroke = 0;
            foreach (char c in part)
            {
                if (c == '*')
                    bold++;
                else if (c == '_')
                    italic++;
                else if (c == '~')
                    stroke++;
            }
            return italic % 2 == 0 && bold % 2 == 0 && stroke % 2 == 0;
        }
        private string MakeMarkdownSafe(string part)
        {
            string res = "";
            foreach (char c in part)
                if (c != '*' && c != '~' && c != '_')
                    res += c;
            return res;
        }
    }
}
