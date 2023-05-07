using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Extensions;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs
{
    public class DefaultMenuManager
    {
        public BotManager Owner { get; set; }
        public List<IPageWrap> Pages { get; set; }

        // openMenu;target;sender
        public BotArgedCallback<MenuNavigatorArg> OpenMenuCallabck { get; set; }

        public DefaultMenuManager(BotManager owner)
        {
            Owner = owner;
            Pages = new();
            OpenMenuCallabck = new BotArgedCallback<MenuNavigatorArg>("", "{open menu}", HandleMenuAsync);
        }

        private async Task HandleMenuAsync(IBotAction<SignedCallbackUpdate> trigger, SignedCallbackUpdate update)
        {
            DefaultArgsSerilalizerService x = new();
            var id = x.Deserialize<MenuNavigatorArg>(update.Data[(update.Data.IndexOf(';') + 1)..]).Value?.RequestedPage;
            IPageWrap? _page = Pages.Find(x => x.PageID == id);

            await PushMenuPageAsync(_page, update);
        }

        private async Task PushMenuPageAsync(IPageWrap page, SignedCallbackUpdate update)
        {
            await Owner.DelieveryService.ReplyToSender(page, update);
        }

        public static string BuildMenuCallback(IPageWrap sender, IPageWrap target)
            => $"OpenMenuPage;{target.PageID};{sender.PageID}";
    }
}