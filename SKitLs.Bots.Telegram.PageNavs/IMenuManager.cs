using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Extensions;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs
{
    public interface IMenuManager : IOwnerCompilable
    {
        public string OpenPageCallBase { get; }
        public void ApplyFor(IActionManager<IBotAction<SignedCallbackUpdate>, SignedCallbackUpdate> callbackManager);

        public IPageWrap? GetPage(IPageWrap page);
        public IPageWrap? GetPage(string pageID);
        public bool IsDefined(IPageWrap page);
        public bool IsDefined(string pageId);
        public void Define(IPageWrap item);

        public BotArgedCallback<MenuNavigatorArg> GetMenuCallabck();
        public Task HandleMenuAsync(IBotAction<SignedCallbackUpdate> trigger, SignedCallbackUpdate update);
        public Task PushMenuPageAsync(IPageWrap? caller, IPageWrap page, ISignedUpdate update);
    }
}