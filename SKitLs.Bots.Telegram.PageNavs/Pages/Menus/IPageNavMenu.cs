using SKitLs.Bots.Telegram.Core.Interactions;

namespace SKitLs.Bots.Telegram.PageNavs.Pages.Menus
{
    /// <summary>
    /// A specialized interface that provides methods for representing a page's menu,
    /// with integrated navigation functionality such as specialized actions or a navigation list.
    /// <para/>
    /// For the default implementation, see: <see cref="PageNavMenu"/>.
    /// </summary>
    public interface IPageNavMenu : IPageMenu
    {
        /// <summary>
        /// Adds links to the specified <paramref name="pages"/> to the menu.
        /// </summary>
        /// <param name="pages">The array of pages that the menu should link to.</param>
        public void PathTo(params IBotPage[] pages);

        /// <summary>
        /// Tries to remove a certain page from the menu's navigation links and returns the result of the attempt.
        /// </summary>
        /// <param name="page">The page to remove.</param>
        /// <returns><see langword="true"/> if an item was found and removed; otherwise, <see langword="false"/>.</returns>
        public bool TryRemove(IBotPage page);

        /// <summary>
        /// Adds a special "Exit" button that refreshes the user's session, setting <paramref name="page"/> as a new root page.
        /// </summary>
        /// <param name="page">The page that the "Exit" button should lead to.</param>
        public void ExitTo(IBotPage? page);

        /// <summary>
        /// Adds a new non-navigation action with certain <paramref name="actionData"/> to the inline menu.
        /// </summary>
        /// <param name="actionData">The labeled data of a callback to add.</param>
        public void AddAction(LabeledData actionData);
    }
}