using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Defaults;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.PageNavs.Args;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs
{
    /// <summary>
    /// An interface that provides methods of inline message navigation via callbacks and <see cref="IBotPage"/> pages.
    /// As a manager: collects, stores and provides access to preset menu pages.
    /// As a service: handles menu updates realized via callbacks.
    /// <para>
    /// Add-on architecture level. Access via <see cref="BotManager.ResolveService{T}"/>.
    /// </para>
    /// <para>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IDebugNamed"/>.
    /// </para>
    /// <para>
    /// Requires: <see cref="IApplicant{T}"/> for <see cref="IActionManager{TUpdate}"/> with <see cref="SignedCallbackUpdate"/>
    /// to define navigation push-back callbacks.
    /// </para>
    /// <para>
    /// For default realization see: <see cref="DefaultMenuManager"/>.
    /// </para>
    /// </summary>
    public interface IMenuManager : IOwnerCompilable, IDebugNamed, IApplicant<IActionManager<SignedCallbackUpdate>>
    {
        /// <summary>
        /// A system callback name, used for <see cref="OpenPageCallback"/>.
        /// </summary>
        public static string OpenCallBase => "OpenMenuPage";
        /// <summary>
        /// A system callback name, used for <see cref="BackCallback"/>.
        /// </summary>
        public static string BackCallBase => "BackMenuPage";

        /// <summary>
        /// System callback that represents "Open Menu" request.
        /// Opens requested page and pushes it into user's session navigation list.
        /// <para>
        /// Requires a <see cref="StringWrapper"/> argument that determines a certain page's id (<c>pageId</c>),
        /// that should be opened.
        /// </para>
        /// </summary>
        public IArgedAction<NavigationArgs, SignedCallbackUpdate> OpenPageCallback { get; }
        /// <summary>
        /// System callback that represents "Back" request.
        /// Gets previous page from user's session data and opens it.
        /// <para>
        /// Doesn't need any arguments to use.
        /// </para>
        /// </summary>
        public IBotAction<SignedCallbackUpdate> BackCallback { get; }

        /// <summary>
        /// Pushes <paramref name="page"/> to a certain user's navigation data.
        /// </summary>
        /// <param name="page">Page to pus.h</param>
        /// <param name="senderId">User's id.</param>
        /// <param name="messageId">Message's that host menu id.</param>
        public void Push(IBotPage page, long senderId, int messageId);
        /// <summary>
        /// Gets the latest page that sender has opened without removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="senderId">User's id.</param>
        /// <returns>The latest opened page.</returns>
        public IBotPage Peek(long senderId);
        /// <summary>
        /// Tries to get the latest page that sender has opened without removing it from the navigation history.
        /// </summary>
        /// <param name="senderId">User's id.</param>
        /// <returns>The latest opened page or <see cref="" langword="null"/> if it doesn't exist.</returns>
        public IBotPage? TryPeek(long senderId);
        /// <summary>
        /// Gets the latest page that sender has opened, removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="senderId">User's id.</param>
        /// <returns>The latest opened page.</returns>
        public IBotPage Pop(long senderId);
        /// <summary>
        /// Tries to get the latest page that sender has opened, removing it from the navigation history.
        /// </summary>
        /// <param name="senderId">User's id.</param>
        /// <returns>The latest opened page or <see cref="" langword="null"/> if it doesn't exist.</returns>
        public IBotPage? TryPop(long senderId);

        /// <summary>
        /// Determines whether a page with <paramref name="pageId"/> id is defined.
        /// </summary>
        /// <param name="pageId">Page's id.</param>
        /// <returns><see langword="true"/> if page exists. Otherwise <see langword="false"/>.</returns>
        public bool IsDefined(string pageId);
        /// <summary>
        /// Saves a new page data into the internal storage, that can be accessed
        /// via <see cref="GetDefined(string)"/> method in future.
        /// </summary>
        /// <param name="item">Page to define by saving it into the internal storage.</param>
        public void Define(IBotPage item);
        /// <summary>
        /// Gets defined via <see cref="Define(IBotPage)"/> page from the internal storage by page's id.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="pageId">Page's id.</param>
        /// <returns>Defined page.</returns>
        public IBotPage GetDefined(string pageId);
        /// <summary>
        /// Tries to get defined via <see cref="Define(IBotPage)"/> page from the internal storage by page's id.
        /// </summary>
        /// <param name="pageId">Page's id.</param>
        /// <returns>Defined page or <see langword="null"/> if doesn't exist.</returns>
        public IBotPage? TryGetDefined(string pageId);

        /// <summary>
        /// Asyncroniously pushes a certain menu <paramref name="page"/> to the chat that has requested it
        /// with a certain signed <paramref name="update"/>.
        /// Based on the update's type, overrides an existing message or generates a new one.
        /// If <paramref name="refresh"/> is <see langword="true"/>, sets <paramref name="page"/> as a new root one.
        /// </summary>
        /// <param name="page">Page to push.</param>
        /// <param name="update">An incoming updated.</param>
        /// <param name="refresh">Determines whether pushing <paramref name="page"/> should be set as a new root one.</param>
        public Task PushPageAsync(IBotPage page, ISignedUpdate update, bool refresh = false);
    }
}