using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    public class PageWrap : IPageWrap
    {
        public string PageID { get; set; }
        public bool LockId { get; set; } = false;

        public IOutputMessage Source { get; set; }
        public int EditMessageId { get; set; }
        public IPageMenu? Menu { get; set; }

        public string Label { get; set; }

        public PageWrap(string pageId, string label, IOutputMessage message)
        {
            PageID = pageId.ToLower();
            Label = label;
            Source = message;
        }

        public string GetLabel() => Label;
        public IOutputMessage BuildMessage(IPageWrap? previous)
        {
            var mes = (IOutputMessage)Source.Clone();
            mes.Menu = Menu?.Build(previous, this);
            return mes;
        }
        public bool TryUpdatePageID(string text, bool append = false)
        {
            if (LockId) return false;

            PageID = append
            ? $"{PageID}{text}"
            : $"{text}{PageID}";
            return true;
        }

        public override string ToString() => PageID;
    }
}