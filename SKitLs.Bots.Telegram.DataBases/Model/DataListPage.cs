using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model
{
    public class DataListPage : IBotPage
    {
        public string PageId => "";
        public IOutputMessage Source => throw new NotImplementedException();
        public IPageMenu? Menu => throw new NotImplementedException();

        public IOutputMessage BuildMessage(IBotPage? previous, ISignedUpdate update)
        {
            throw new NotImplementedException();
        }

        public string GetLabel(ISignedUpdate upadte)
        {
            throw new NotImplementedException();
        }

        public string GetPacked() => PageId;
    }
}