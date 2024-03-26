using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Args;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Settings;

namespace SKitLs.Bots.Telegram.PageNavs.Callbacks
{
    /// <summary>
    /// Represents the default implementation of a callback for opening menu pages.
    /// </summary>
    public class DefaultOpenPageCallback : BotArgedCallback<NavigationArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOpenPageCallback"/> class with the specified default label (<see cref="PNSettings.OpenCallBase"/>).
        /// </summary>
        /// <param name="defaultLabel">The default label for the open button.</param>
        public DefaultOpenPageCallback(string defaultLabel = "{open menu}") : base(PNSettings.OpenCallBase, defaultLabel, null!)
        {
            ArgAction = OpenMenuAsync;
        }

        /// <summary>
        /// Handles the action of opening a menu page asynchronously.
        /// </summary>
        /// <param name="args">The navigation arguments.</param>
        /// <param name="update">The signed callback update.</param>
        private async Task OpenMenuAsync(NavigationArgs args, SignedCallbackUpdate update)
        {
            var menuService = update.Owner.ResolveService<IMenuService>();
            if (menuService.SessionsManager.CheckSession(update))
                await menuService.PushPageAsync(args.Page, update, args.Refresh);
            else
                await MenusHelper.HandleSessionExpiredAsync(update);
        }
    }
}