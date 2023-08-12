using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Args;
using SKitLs.Bots.Telegram.PageNavs.Model;

namespace SKitLs.Bots.Telegram.PageNavs.Prototype
{
    /// <summary>
    /// Represents an interface for managing menu pages and handling inline message navigation through callbacks.
    /// Serves as both a manager (collecting, storing, and providing access to preset menu pages)
    /// and a service (handling menu updates via callbacks).
    /// <para>
    /// Add-on architecture level. Access via <see cref="BotManager.ResolveService{T}"/>.
    /// </para>
    /// <para>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IDebugNamed"/>.
    /// </para>
    /// <para>
    /// <b>Requires:</b> <see cref="IApplicant{T}"/> for <see cref="IActionManager{TUpdate}"/> with <see cref="SignedCallbackUpdate"/>
    /// to define navigation push-back callbacks.
    /// </para>
    /// <para>
    /// For default implementation, see: <see cref="DefaultMenuManager"/>.
    /// </para>
    /// </summary>
    public interface IMenuManager : IOwnerCompilable, IDebugNamed, IApplicant<IActionManager<SignedCallbackUpdate>>
    {
        /// <summary>
        /// Represents the system callback for "Open Menu" requests.
        /// Opens the requested page and adds it to the user's session navigation list.
        /// <para>
        /// Requires a <see cref="NavigationArgs"/> argument to determine paging data.
        /// </para>
        /// </summary>
        public IArgedAction<NavigationArgs, SignedCallbackUpdate> OpenPageCallback { get; }

        /// <summary>
        /// Represents the system callback for "Back" requests.
        /// Retrieves the previous page from the user's session data and opens it.
        /// <para>
        /// Does not require any arguments to use.
        /// </para>
        /// </summary>
        public IBotAction<SignedCallbackUpdate> BackCallback { get; }

        /// <summary>
        /// Pushes the <paramref name="page"/> to a certain user's navigation data.
        /// </summary>
        /// <param name="page">The page to push.</param>
        /// <param name="senderId">The user's ID.</param>
        /// <param name="messageId">The ID of the message hosting the menu.</param>
        public void Push(IBotPage page, long senderId, int messageId);

        /// <summary>
        /// Gets the latest page that the sender has opened without removing it from the navigation history.
        /// </summary>
        /// <param name="senderId">The user's ID.</param>
        /// <returns>The latest opened page.</returns>
        public IBotPage Peek(long senderId);

        /// <summary>
        /// Tries to get the latest page that the sender has opened without removing it from the navigation history.
        /// </summary>
        /// <param name="senderId">The user's ID.</param>
        /// <returns>The latest opened page or <see langword="null"/> if it does not exist.</returns>
        public IBotPage? TryPeek(long senderId);

        /// <summary>
        /// Gets the latest page that the sender has opened, removing it from the navigation history.
        /// </summary>
        /// <param name="senderId">The user's ID.</param>
        /// <returns>The latest opened page.</returns>
        public IBotPage Pop(long senderId);

        /// <summary>
        /// Tries to get the latest page that the sender has opened, removing it from the navigation history.
        /// </summary>
        /// <param name="senderId">The user's ID.</param>
        /// <returns>The latest opened page or <see langword="null"/> if it does not exist.</returns>
        public IBotPage? TryPop(long senderId);

        /// <summary>
        /// Determines whether a page with the given <paramref name="pageId"/> exists.
        /// </summary>
        /// <param name="pageId">The page's ID.</param>
        /// <returns><see langword="true"/> if the page exists; otherwise, <see langword="false"/>.</returns>
        public bool IsDefined(string pageId);
        
        /// <summary>
        /// Saves a new page data into the internal storage, which can be accessed
        /// via the <see cref="GetDefined(string)"/> method in the future.
        /// </summary>
        /// <param name="item">The page to define by saving it into the internal storage.</param>
        public void Define(IBotPage item);

        /// <summary>
        /// Gets a defined page from the internal storage using the page's ID, as defined via <see cref="Define(IBotPage)"/>.
        /// Throws an exception if that page does not exist.
        /// </summary>
        /// <param name="pageId">The page's ID.</param>
        /// <returns>The defined page.</returns>
        public IBotPage GetDefined(string pageId);

        /// <summary>
        /// Tries to get a defined page from the internal storage using the page's ID, as defined via <see cref="Define(IBotPage)"/>.
        /// </summary>
        /// <param name="pageId">The page's ID.</param>
        /// <returns>The defined page or <see langword="null"/> if it does not exist.</returns>
        public IBotPage? TryGetDefined(string pageId);

        /// <summary>
        /// Asynchronously pushes a certain menu <paramref name="page"/> to the chat that has requested it
        /// with a certain signed <paramref name="update"/>.
        /// Based on the update's type, it either overrides an existing message or generates a new one.
        /// If <paramref name="refresh"/> is <see langword="true"/>, sets <paramref name="page"/> as a new root page.
        /// </summary>
        /// <param name="page">The page to push.</param>
        /// <param name="update">The incoming update.</param>
        /// <param name="refresh">Determines whether pushing <paramref name="page"/> should be set as a new root page.</param>
        public Task PushPageAsync(IBotPage page, ISignedUpdate update, bool refresh = false);
    }
}