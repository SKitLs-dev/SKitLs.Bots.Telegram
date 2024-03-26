using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.Services;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Args;
using SKitLs.Bots.Telegram.PageNavs.Pages;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// Provides an interface for managing menu pages and handling inline message navigation through callbacks.
    /// Serves as both a manager (collecting, storing, and providing access to preset menu pages)
    /// and a service (handling menu updates via callbacks).
    /// <para/>
    /// This interface operates at the add-on architecture level and can be accessed via <see cref="BotManager.ResolveService{T}"/>.
    /// </summary>
    /// <remarks>
    /// For the default implementation, refer to: <see cref="MenuService"/>.
    /// </remarks>
    public interface IMenuService : IBotService, IApplicant<IActionManager<SignedCallbackUpdate>>
    {
        /// <summary>
        /// Gets the session manager responsible for managing menu sessions.
        /// </summary>
        public ISessionsManager SessionsManager { get; }

        /// <summary>
        /// Represents the system callback for "Open Menu" requests.
        /// Opens the requested page and adds it to the user's session navigation list.
        /// <para/>
        /// Requires a <see cref="NavigationArgs"/> argument to determine paging data.
        /// </summary>
        public IArgedAction<NavigationArgs, SignedCallbackUpdate> OpenPageCallback { get; }

        /// <summary>
        /// Represents the system callback for "Back" requests.
        /// Retrieves the previous page from the user's session data and opens it.
        /// <para/>
        /// Does not require any arguments to use.
        /// </summary>
        public IBotAction<SignedCallbackUpdate> BackCallback { get; }

        /// <summary>
        /// Determines whether a page with the given <paramref name="pageId"/> exists.
        /// </summary>
        /// <param name="pageId">The ID of the page to check.</param>
        /// <returns><see langword="true"/> if the page exists; otherwise, <see langword="false"/>.</returns>
        public bool IsDefined(string pageId);

        /// <summary>
        /// Saves a new page data into the internal storage.
        /// </summary>
        /// <param name="item">The page to define by saving it into the internal storage.</param>
        public void Define(IBotPage item);

        /// <summary>
        /// Tries to get a defined page from the internal storage.
        /// </summary>
        /// <param name="pageId">The ID of the page to retrieve.</param>
        /// <returns>The defined page or <see langword="null"/> if it does not exist.</returns>
        public IBotPage? TryGetDefined(string pageId);

        /// <summary>
        /// Gets a defined page from the internal storage.
        /// Throws an exception if the page does not exist.
        /// </summary>
        /// <param name="pageId">The ID of the page to retrieve.</param>
        /// <returns>The defined page.</returns>
        public IBotPage GetDefined(string pageId);

        /// <summary>
        /// Asynchronously pushes a menu page to the chat based on the incoming update.
        /// If <paramref name="refresh"/> is <see langword="true"/>, sets <paramref name="page"/> as a new root page.
        /// </summary>
        /// <param name="page">The page to push.</param>
        /// <param name="update">The incoming update.</param>
        /// <param name="refresh">Determines whether pushing <paramref name="page"/> should set it as a new root page.</param>
        public Task PushPageAsync(IBotPage page, ISignedUpdate update, bool refresh = false);
    }
}