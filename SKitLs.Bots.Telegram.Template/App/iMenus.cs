using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model;

namespace SKitLs.Bots.Telegram.Template.App
{
    internal partial class MainApplicant
    {
        private static readonly string MainPageId = "mainMenu";
        private static readonly string SubPageId = "subMenu";

        public DefaultMenuManager GetMenuManager()
        {
            var menuManager = new DefaultMenuManager();

            var mainMenu = new PageNavMenu() { ColumnsCount = 2 };
            var mainPage = new WidgetPage(MainPageId, u => LabelBuilder(MainPageId, u), PageBuilder(MainPageId), mainMenu);

            var subPageMenu = new PageNavMenu();
            subPageMenu.AddAction(ClickMeCallback);
            var subPage = new WidgetPage(SubPageId, u => LabelBuilder(SubPageId, u), SubPageBuilder(SubPageId), subPageMenu);

            mainMenu.PathTo(subPage);

            menuManager.Define(mainPage);
            menuManager.Define(subPage);

            return menuManager;
        }

        private static string LabelBuilder(string menuId, ISignedUpdate update) => update.Owner.ResolveBotString($"app.{menuId}Label");
        private static IOutputMessage PageBuilder(string menuId)
        {
            var message = new OutputMessageText($"app.{menuId}")
            {
                ContentBuilder = async (m, u) => {
                    m.Text = u?.Owner.ResolveBotString(m.Text) ?? m.Text;
                    return await Task.FromResult(m);
                }
            };
            return message;
        }

        private static IOutputMessage SubPageBuilder(string menuId)
        {
            return new OutputMessageText($"app.{menuId}")
            {
                ContentBuilder = async (m, u) =>
                {
                    if (u is ISignedUpdate signed && u.ChatScanner.UsersManager is not null)
                    {
                        var user = (DefaultBotUser?)await u.ChatScanner.UsersManager.GetUserByIdAsync(signed.Sender.TelegramId) ?? new DefaultBotUser(0);
                        m.Text = u?.Owner.ResolveBotString(m.Text, user.TelegramId.ToString()) ?? m.Text;
                    }
                    return await Task.FromResult(m);
                }
            };
        }
    }
}
