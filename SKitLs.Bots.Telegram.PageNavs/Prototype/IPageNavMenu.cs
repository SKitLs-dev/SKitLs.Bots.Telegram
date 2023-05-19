namespace SKitLs.Bots.Telegram.PageNavs.Prototype
{
    public interface IPageNavMenu : IPageMenu
    {
        public void PathTo(params IPageWrap[] page);
        public bool Remove(IPageWrap page);
        public void ExitTo(IPageWrap? page);
    }
}