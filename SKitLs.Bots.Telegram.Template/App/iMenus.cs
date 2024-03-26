using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Pages;
using SKitLs.Bots.Telegram.PageNavs.Pages.Menus;
using SKitLs.Bots.Telegram.Template.View.Menus;

namespace SKitLs.Bots.Telegram.Template.App
{
    internal partial class MainApplicant
    {
        private static readonly string MainPageId = "mainMenu";
        private static readonly string SubPageId = "subMenu";

        public MenuService GetMenuManager()
        {
            var menuManager = new MenuService();
            LocalizedPage.LabelKeyMask = "app.{0}Label";
            LocalizedPage.PageKeyMask = "app.{0}Page";

            var mainMenu = new PageNavMenu() { ColumnsCount = 2 };
            var mainPage = new LocalizedPage(MainPageId, mainMenu);

            var subPageMenu = new PageNavMenu();
            subPageMenu.AddAction(ClickMeCallback);
            var subPage = new LocalizedSubPage(SubPageId, subPageMenu);

            mainMenu.PathTo(subPage);

            menuManager.Define(mainPage);
            menuManager.Define(subPage);

            return menuManager;
        }
    }
}