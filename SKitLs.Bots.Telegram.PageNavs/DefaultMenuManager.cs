using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Extensions;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Extensions;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs
{
    public class DefaultMenuManager : IMenuManager
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public static string OpenCallBase => "OpenMenuPage";
        public static string BackCallBase => "BackMenuPage";
        public string OpenPageCallBase => OpenCallBase;

        public List<IPageWrap> Pages { get; set; } = new();
        public IPageWrap? GetPage(IPageWrap page) => GetPage(page.PageID);
        public IPageWrap? GetPage(string pageID) => Pages.Find(x => x.PageID == pageID);
        public bool IsDefined(IPageWrap page) => GetPage(page) != null;
        public bool IsDefined(string pageId) => GetPage(pageId) != null;
        public void Define(IPageWrap item)
        {
            if (string.IsNullOrEmpty(item.PageID)) throw new Exception();
            if (IsDefined(item)) throw new Exception();
            Pages.Add(item);
        }

        // openMenu;target;sender
        public BotArgedCallback<StringDbto> GetBackCallabck() => new(BackCallBase, "{back menu}", BackMenuAsync);
        private async Task BackMenuAsync(IBotAction<SignedCallbackUpdate> trigger, SignedCallbackUpdate update)
        {
            var argService = (IArgsSerilalizerService)Owner.ResolveService<IArgsSerilalizerService>();
            var args = argService.Deserialize<StringDbto>(update.Data[(update.Data.IndexOf(';') + 1)..], ';').Value;
            IPageWrap? _page = Pages.Find(x => x.PageID == args.Content);

            // PageNotFoundException()
            if (_page is null) throw new Exception();

            await PushMenuPageAsync(null, _page, update);
        }

        public BotArgedCallback<MenuNavigatorArg> GetMenuCallabck() => new(OpenPageCallBase, "{open menu}", HandleMenuAsync);
        public async Task HandleMenuAsync(IBotAction<SignedCallbackUpdate> trigger, SignedCallbackUpdate update)
        {
            var argService = (IArgsSerilalizerService)Owner.ResolveService<IArgsSerilalizerService>();
            var args = argService.Deserialize<MenuNavigatorArg>(update.Data[(update.Data.IndexOf(';') + 1)..], ';').Value;
            IPageWrap? _page = Pages.Find(x => x.PageID == args.RequestedPage);
            IPageWrap? _caller = Pages.Find(x => x.PageID == args.PreviousPage);

            // PageNotFoundException()
            if (_page is null || _caller is null) throw new Exception();

            await PushMenuPageAsync(_caller, _page, update);
        }

        public async Task PushMenuPageAsync(IPageWrap? caller, IPageWrap page, ISignedUpdate update)
        {
            IOutputMessage message = page.BuildMessage(caller);

            if (update is SignedCallbackUpdate callback)
                await Owner.DelieveryService.ReplyToSender(new EditWrapper(message, callback.TriggerMessageId), callback);
            else await Owner.DelieveryService.ReplyToSender(message, update);
        }

        public static string BuildMenuCallback(IPageWrap sender, IPageWrap target)
            => $"{OpenCallBase};{target.PageID};{sender.PageID}";
        public static string BuildBackCallback(IPageWrap target)
            => $"{BackCallBase};{target.PageID}";

        public void ApplyFor(IActionManager<IBotAction<SignedCallbackUpdate>, SignedCallbackUpdate> callbackManager)
        {
            callbackManager.Actions.Add(GetMenuCallabck());
            callbackManager.Actions.Add(GetBackCallabck());
        }
    }
}