using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    public class PageWrap : IPageWrap
    {
        public string PageID { get; set; }

        public IOutputMessage Source { get; set; }
        public int EditMessageId { get; set; }
        public ParseMode? ParseMode { get; set; }

        public IPageMenu? Menu { get; set; }
        private IReplyMarkup _markup;
        public IReplyMarkup? Markup => _markup;

        public string Label { get; set; }

        public PageWrap(string pageId, string label, IOutputMessage message)
        {
            PageID = pageId;
            Label = label;
            Source = message;
            //Body = $"SKitLs Menu {pageId}";
        }

        public string GetLabel() => Label;
        public IOutputMessage BuildMessage(IPageWrap previos)
        {
            Source.Markup = Menu.Build(previos, this);
            return Source;
        }
    }
}