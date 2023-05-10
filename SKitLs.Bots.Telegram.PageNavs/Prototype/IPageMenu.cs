using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.PageNavs.Prototype
{
    public interface IPageMenu
    {
        public int ColumnsCount { get; }

        public void PathTo(IPageWrap page);
        public bool Remove(IPageWrap page);
        public void ExitTo(IPageWrap? page);

        public InlineKeyboardMarkup Build(IPageWrap? previous, IPageWrap owner);
    }
}
