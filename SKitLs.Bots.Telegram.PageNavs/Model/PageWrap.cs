using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    public class PageWrap : IPageWrap
    {
        public string PageID { get; set; }

        public IOutputMessage Source { get; set; }
        public int EditMessageId { get; set; }
        public IPageMenu? Menu { get; set; }

        public string Label { get; set; }

        public PageWrap(string pageId, string label, IOutputMessage message)
        {
            PageID = pageId;
            Label = label;
            Source = message;
            //Body = $"SKitLs Menu {pageId}";
        }

        public string GetLabel() => Label;
        public IOutputMessage BuildMessage(IPageWrap? previous)
        {
            var mes = (IOutputMessage)Source.Clone();
            mes.Markup = Menu?.Build(previous, this);
            return mes;
        }
    }
}