using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Users;
using SKitLs.Bots.Telegram.PageNavs.Pages;
using SKitLs.Bots.Telegram.PageNavs.Pages.Menus;

namespace SKitLs.Bots.Telegram.Template.View.Menus
{
    public class LocalizedSubPage(string pageId, IPageMenu? menu = null) : LocalizedPage(pageId, menu)
    {
        public override async Task<string[]> ResolveBodyFormatParameters(ISignedUpdate update)
        {
            var format = new List<string>();
            if (update.ChatScanner.UsersManager is not null)
            {
                var user = (DefaultBotUser?)await update.ChatScanner.UsersManager.GetUserByIdAsync(update.Sender.TelegramId) ?? new DefaultBotUser(0, false, "en", "Unnamed");
                format.Add(user.TelegramId.ToString());
            }
            return format.ToArray();
        }
    }
}