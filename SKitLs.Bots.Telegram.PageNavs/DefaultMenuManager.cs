using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
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
        public Action<object, BotManager>? OnCompilation => OnReflectiveCompile;

        public static string OpenCallBase => "OpenMenuPage";
        public static string BackCallBase => "BackMenuPage";
        public string OpenPageCallBase => OpenCallBase;

        public List<IPageWrap> Pages { get; set; } = new();
        public IPageWrap? TryGetPage(IPageWrap page) => TryGetPage(page.PageID);
        public IPageWrap? TryGetPage(string pageID) => Pages.Find(x => x.PageID == pageID);
        public IPageWrap GetPage(IPageWrap page) => GetPage(page.PageID);
        public IPageWrap GetPage(string pageID) => TryGetPage(pageID) ?? throw new Exception();
        public bool IsDefined(IPageWrap page) => TryGetPage(page) != null;
        public bool IsDefined(string pageId) => TryGetPage(pageId) != null;
        public void Define(IPageWrap item)
        {
            if (string.IsNullOrEmpty(item.PageID)) throw new Exception();
            if (IsDefined(item)) throw new Exception();
            Pages.Add(item);
        }

        // openMenu;target;sender
        public BotArgedCallback<StringDbto> GetBackCallabck() => new(BackCallBase, "{back menu}", BackMenuAsync);
        private async Task BackMenuAsync(IBotAction<SignedCallbackUpdate> trigger, StringDbto args, SignedCallbackUpdate update)
        {
            IPageWrap? _page = Pages.Find(x => x.PageID == args.Content);

            // PageNotFoundException()
            if (_page is null) throw new Exception();

            await PushMenuPageAsync(null, _page, update);
        }

        public BotArgedCallback<MenuNavigatorArg> GetMenuCallabck() => new(OpenPageCallBase, "{open menu}", HandleMenuAsync);
        public async Task HandleMenuAsync(IBotAction<SignedCallbackUpdate> trigger, MenuNavigatorArg args, SignedCallbackUpdate update)
        {
            IPageWrap? _page = Pages.Find(x => x.PageID == args.RequestedPage);
            IPageWrap? _caller = Pages.Find(x => x.PageID == args.PreviousPage);

            // PageNotFoundException()
            if (_page is null || _caller is null) throw new Exception();

            await PushMenuPageAsync(_caller, _page, update);
        }

        public async Task PushMenuPageAsync(IPageWrap? caller, IPageWrap page, ISignedUpdate update)
        {
            IOutputMessage message = page.BuildMessage(caller);
            var resMes = message.FormattedClone?.Invoke(message, update) ?? message;

            if (update is SignedCallbackUpdate callback)
                await Owner.DelieveryService.ReplyToSender(new EditWrapper(resMes, callback.TriggerMessageId), callback);
            else await Owner.DelieveryService.ReplyToSender(resMes, update);
        }

        public static string BuildMenuCallback(IPageWrap sender, IPageWrap target) => $"{OpenCallBase};{target.PageID};{sender.PageID}";
        public static string BuildBackCallback(IPageWrap target) => $"{BackCallBase};{target.PageID}";

        private void OnReflectiveCompile(object sender, BotManager bm)
        {
            var pageRule = new ConvertRule<IPageWrap>(pid => IsDefined(pid) ? ConvertResult<IPageWrap>.OK(TryGetPage(pid)!) : ConvertResult<IPageWrap>.Incorrect($"Page with id {pid} is not defined."));
            bm.ResolveService<IArgsSerilalizerService>().AddRule(pageRule);
        }
        public void ApplyFor(IActionManager<SignedCallbackUpdate> callbackManager)
        {
            callbackManager.AddSafely(GetMenuCallabck());
            callbackManager.AddSafely(GetBackCallabck());
        }
    }
}