using SKitLs.Bots.Telegram.Core.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Settings;

namespace SKitLs.Bots.Telegram.PageNavs.Callbacks
{
    /// <summary>
    /// Represents the default implementation of a callback for navigating back to the previous menu page.
    /// </summary>
    public class DefaultBackCallback : DefaultCallback
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBackCallback"/> class with the specified default label (<see cref="PNSettings.BackCallBase"/>).
        /// </summary>
        /// <param name="defaultLabel">The default label for the back button.</param>
        public DefaultBackCallback(string defaultLabel = "{back menu}") : base(PNSettings.BackCallBase, defaultLabel, null!)
        {
            Action = BackMenuAsync;
        }

        /// <summary>
        /// Handles the action of navigating back to the previous menu page asynchronously.
        /// </summary>
        /// <param name="update">The signed callback update.</param>
        private async Task BackMenuAsync(SignedCallbackUpdate update)
        {
            var menuService = update.Owner.ResolveService<IMenuService>();

            // Session invalid => Handle Expired
            // - - - -
            // Attempt to delete current page.
            // if null => Handle Expired
            if (!menuService.SessionsManager.CheckSession(update) || menuService.SessionsManager.TryPop(update) is null)
            {
                await MenusHelper.HandleSessionExpiredAsync(update);
                return;
            }

            // Attempt to extract previous page from a history. / will be pushed automatically
            // if null (nothing to open) => Handle Expired
            var page = menuService.SessionsManager.TryPop(update);
            if (page is null)
            {
                await MenusHelper.HandleSessionExpiredAsync(update);
                return;
            }
            await menuService.PushPageAsync(page, update);
        }
    }
}