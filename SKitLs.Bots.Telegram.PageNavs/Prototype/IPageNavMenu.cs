using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.PageNavs.Model;

namespace SKitLs.Bots.Telegram.PageNavs.Prototype
{
    /// <summary>
    /// A specified interface that provides methods for the representation of a page's menu,
    /// with integrated navigation functionality such as specialized actions or navigations list.
    /// <para>
    /// For default realization see: <see cref="PageNavMenu"/>.
    /// </para>
    /// </summary>
    public interface IPageNavMenu : IPageMenu
    {
        /// <summary>
        /// Adds links to specified <paramref name="pages"/> to the menu.
        /// </summary>
        /// <param name="pages">The array of pages that menu should link to.</param>
        public void PathTo(params IBotPage[] pages);

        /// <summary>
        /// Tries to remove a certain page from menu's navigation links and returns the result of an attempt.
        /// </summary>
        /// <param name="page">Page to remove.</param>
        /// <returns><see langword="true"/> if an item was found and removed. Otherwise <see langword="false"/>.</returns>
        public bool TryRemove(IBotPage page);

        /// <summary>
        /// Adds special "Exit" Button, that refreshes user's session setting <paramref name="page"/> as a new root page.
        /// </summary>
        /// <param name="page">Page that "Exit" button should lead to.</param>
        public void ExitTo(IBotPage? page);

        /// <summary>
        /// Adds new non-navigation action with a certain <paramref name="actionData"/> to the inline menu.
        /// </summary>
        /// <param name="actionData">Labeled data of a callback to add.</param>
        public void AddAction(LabeledData actionData);
    }
}