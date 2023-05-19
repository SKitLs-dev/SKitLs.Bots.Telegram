using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model
{
    public class DataListPage : IPageWrap
    {
        public string PageID => throw new NotImplementedException();
        public IOutputMessage Source => throw new NotImplementedException();

        public IOutputMessage BuildMessage(IPageWrap? previous)
        {
            throw new NotImplementedException();
        }

        public string GetLabel()
        {
            throw new NotImplementedException();
        }

        public bool TryUpdatePageID(string text, bool append = false)
        {
            throw new NotImplementedException();
        }
    }
}
