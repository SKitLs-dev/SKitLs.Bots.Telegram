using SKitLs.Bots.Telegram.PageNavs.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    public class PageMenu : IPageMenu
    {
        public int ColumnsCount { get; set; } = 1;

        public List<IPageWrap> PagesLinks { get; private set; }
        public bool EnableBackButton { get; set; } = true;
        public IPageWrap? ExitButton { get; private set; }

        public PageMenu()
        {
            PagesLinks = new();
        }

        public void PathTo(IPageWrap page) => PagesLinks.Add(page);
        public bool Remove(IPageWrap page) => PagesLinks.Remove(page);
        public void ExitTo(IPageWrap? page) => ExitButton = page;

        public InlineKeyboardMarkup Build(IPageWrap? previous, IPageWrap owner)
        {
            List<List<InlineKeyboardButton>> data = new();
            int ti = 0;
            List<InlineKeyboardButton> temp = new();
            for (int i = 0; i < PagesLinks.Count; i++)
            {
                var next = PagesLinks[i];
                temp.Add(InlineKeyboardButton.WithCallbackData(next.GetLabel(),
                    DefaultMenuManager.BuildMenuCallback(owner, next)));
                ti++;
                if (ti % ColumnsCount == 0)
                {
                    data.Add(temp);
                    temp = new();
                }
            }
            if (ti % ColumnsCount != 0) data.Add(temp);

            if (previous is not null)
                data.Add(new()
                {
                    InlineKeyboardButton.WithCallbackData("<< Назад",
                        DefaultMenuManager.BuildBackCallback(previous))
                });
            return new(data);
        }
    }
}

//  Page p1 = new(id, "Label");
//  Page p2 = new(id, "Label");
//  Page p3 = new(id, "Label");
//  Page p4 = new(id, "Label");
//
//  InlineMenu p1_menu = new();
//
//  NavMenu m1 = new NavMenu(2, new() { p1, p2, p3 } );
//  NavMenu m2 = new NavMenu(2, new() { p2, p4 } );
//
//  p1_menu.AddMenu(m1);
//  p1_menu.BackButton = null;
//  p1_menu.CloseButton = null;
//  p1_menu.Insert(m2, 1);
//
//  p1.SetMenu(p1_menu);
//
//  CallsMenu cm1 = new(1, new() { callback1, callback2... });
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//