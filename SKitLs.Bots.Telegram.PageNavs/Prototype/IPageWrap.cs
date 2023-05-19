using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Prototype
{
    public interface IPageWrap
    {
        public string PageID { get; }
        public IOutputMessage Source { get; }

        public IOutputMessage BuildMessage(IPageWrap? previous);
        public bool TryUpdatePageID(string text, bool append = false);
        public string GetLabel();
    }
}