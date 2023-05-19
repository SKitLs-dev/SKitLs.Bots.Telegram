using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    public class PageNavMenu : IPageNavMenu
    {
        public int ColumnsCount { get; set; } = 1;

        public List<IPageWrap> PagesLinks { get; private set; } = new();
        public bool EnableBackButton { get; set; } = true;
        public IPageWrap? ExitButton { get; private set; }

        public void PathTo(params IPageWrap[] pages) => pages.ToList().ForEach(p => PagesLinks.Add(p));
        public bool Remove(IPageWrap page) => PagesLinks.Remove(page);
        public void ExitTo(IPageWrap? page) => ExitButton = page;

        public IMesMenu Build(IPageWrap? previous, IPageWrap owner)
        {
            var res = new PairedInlineMenu()
            {
                ColumnsCount = ColumnsCount,
            };
            PagesLinks.ForEach(page => res.Add(
                page.GetLabel() + " >", DefaultMenuManager.BuildMenuCallback(owner, page)));

            if (previous is not null)
                res.Add("<< Назад", DefaultMenuManager.BuildBackCallback(previous), true);

            return res;
        }
    }
}