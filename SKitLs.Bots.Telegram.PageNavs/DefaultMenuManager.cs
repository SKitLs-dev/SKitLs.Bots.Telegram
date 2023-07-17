using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.PageNavs.Args;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs
{
    /// <summary>
    /// Default realization of the <see cref="IMenuManager"/> interface that provides methods of inline message navigation.
    /// Realised via <see cref="PageSessionData"/>, that provides simple navigation data stack-storage for only one message.
    /// Enables two ways of navigation: forward and backward by allowing to open a new page via <see cref="OpenPageCallabck"/>
    /// or rollback to a previous one via <see cref="BackCallabck"/>.
    /// <para>
    /// Allows to store menu pages' data, providing access to it, and handle special menu updates, released via callbacks.
    /// </para>
    /// <para>
    /// Add-on architecture level. Access via <see cref="BotManager.ResolveService{T}"/>.
    /// </para>
    /// <para>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IDebugNamed"/>.
    /// </para>
    /// <para>
    /// Requires: <see cref="IApplicant{T}"/> for <see cref="IActionManager{TUpdate}"/> with <see cref="SignedCallbackUpdate"/>
    /// to define push-back callbacks.
    /// </para>
    /// </summary>
    public class DefaultMenuManager : IMenuManager
    {
        public string? DebugName { get; set; }

        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => OnReflectiveCompile;

        public static string SessionExpiredKey { get; set; } = "pn.display.SessionExpired";

        /// <summary>
        /// Creates a new instance of a <see cref="DefaultMenuManager"/> with specified data.
        /// </summary>
        /// <param name="debugName">Implemention of <see cref="IDebugNamed"/>.</param>
        public DefaultMenuManager(string? debugName = null) => DebugName = debugName;

        #region Sessions and navigation data.
        /// <summary>
        /// An internal storage that provides access to all saved navigation sessions.
        /// </summary>
        private Dictionary<long, PageSessionData> Navigations { get; } = new();
        /// <summary>
        /// Detects whether a session for a certain update exists and valid.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns><see langword="true"/> if <paramref name="update"/> is a <see cref="SignedCallbackUpdate"/>,
        /// session exists and its <see cref="PageSessionData.MessageId"/>
        /// is equal to the <paramref name="update"/>'s <see cref="SignedCallbackUpdate.TriggerMessageId"/>.
        /// Otherwise <see langword="false"/>.</returns>
        private bool CheckSession(ISignedUpdate update) => update is SignedCallbackUpdate callback && CheckSession(callback);
        /// <summary>
        /// Detects whether a session for a certain update exists and valid.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns><see langword="true"/> if session exists and its <see cref="PageSessionData.MessageId"/>
        /// is equal to the <paramref name="update"/>'s <see cref="SignedCallbackUpdate.TriggerMessageId"/>.
        /// Otherwise <see langword="false"/>.</returns>
        private bool CheckSession(SignedCallbackUpdate update)
            => CheckSession(update.Sender.TelegramId, update.TriggerMessageId);
        /// <summary>
        /// Detects whether a session for the specified data exists and valid.
        /// </summary>
        /// <param name="userId">User's id, to check its session.</param>
        /// <param name="messageId">Message's id, to check session for.</param>
        /// <returns><see langword="true"/> if session exists and its <see cref="PageSessionData.MessageId"/>
        /// is equal to the <paramref name="messageId"/>.
        /// Otherwise <see langword="false"/>.</returns>
        private bool CheckSession(long userId, int messageId)
            => Navigations.ContainsKey(userId)
            && Navigations[userId].MessageId == messageId;

        /// <summary>
        /// Gets user's menu session by an incoming signed update for a certain message.
        /// If one doesn't exist, generates and returns a new one.
        /// </summary>
        /// <param name="update">An incoming signed update.</param>
        /// <param name="messageId">Message's id.</param>
        /// <returns>An existing or a new one <see cref="PageSessionData"/> for a certain user and message.</returns>
        public PageSessionData GetSession(ISignedUpdate update, int messageId) => GetSession(update.Sender, messageId);
        /// <summary>
        /// Gets user's menu session for a certain message.
        /// If one doesn't exist, generates and returns a new one.
        /// </summary>
        /// <param name="sender">A certain sender.</param>
        /// <param name="messageId">Message's id.</param>
        /// <returns>An existing or a new one <see cref="PageSessionData"/> for a certain user and message.</returns>
        public PageSessionData GetSession(IBotUser sender, int messageId) => GetSession(sender.TelegramId, messageId);
        /// <summary>
        /// Gets user's menu session by his id for a certain message.
        /// If one doesn't exist, generates and returns a new one.
        /// </summary>
        /// <param name="senderId">User's id.</param>
        /// <param name="messageId">Message's id.</param>
        /// <returns>An existing or a new one <see cref="PageSessionData"/> for a certain user and message.</returns>
        public PageSessionData GetSession(long senderId, int messageId)
        {
            if (CheckSession(senderId, messageId))
                return Navigations[senderId];

            var res = new PageSessionData(senderId, messageId);
            if (!Navigations.ContainsKey(senderId))
                Navigations.Add(senderId, res);
            if (Navigations[senderId].MessageId != messageId)
                Navigations[senderId] = res;
            return res;
        }
        #endregion

        #region Sessions manipulation.
        /// <summary>
        /// Pushes <paramref name="page"/> to a certain <see cref="PageSessionData"/> <paramref name="session"/>.
        /// </summary>
        /// <param name="page">Page to push.</param>
        /// <param name="session">Session to push page to.</param>
        public void Push(IBotPage page, PageSessionData session) => Push(page, session.OwnerId, session.MessageId);
        /// <summary>
        /// Pushes <paramref name="page"/> to a certain user's navigation data.
        /// </summary>
        /// <param name="page">Page to push.</param>
        /// <param name="update">An incoming update.</param>
        /// <param name="messageId">Message's that host menu id.</param>
        public void Push(IBotPage page, ISignedUpdate update, int messageId) => Push(page, update.Sender, messageId);
        /// <summary>
        /// Pushes <paramref name="page"/> to a certain user's navigation data.
        /// </summary>
        /// <param name="page">Page to open.</param>
        /// <param name="sender">A certain sender.</param>
        /// <param name="messageId">Message's that host menu id.</param>
        public void Push(IBotPage page, IBotUser sender, int messageId) => Push(page, sender.TelegramId, messageId);
        public void Push(IBotPage page, long userId, int mesId) => GetSession(userId, mesId).Push(page);
        /// <summary>
        /// Gets the latest page that update's sender has opened without removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/>
        /// for <paramref name="senderId"/> doesn't exist.</exception>
        public IBotPage Peek(ISignedUpdate update) => Peek(update.Sender);
        /// <summary>
        /// Gets the latest page that sender has opened without removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="sender">A certain sender.</param>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/>
        /// for <paramref name="senderId"/> doesn't exist.</exception>
        public IBotPage Peek(IBotUser sender) => Peek(sender.TelegramId);
        /// <summary>
        /// Gets the latest page that sender has opened without removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="senderId">User's id.</param>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/>
        /// for <paramref name="senderId"/> doesn't exist.</exception>
        public IBotPage Peek(long senderId) => TryPeek(senderId)
            ?? throw new NotDefinedException(GetType(), typeof(PageSessionData), senderId.ToString());
        /// <summary>
        /// Tries to get the latest page that update's sender has opened without removing it from the navigation history.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>The latest opened page or <see cref="" langword="null"/> if it doesn't exist.</returns>
        public IBotPage? TryPeek(ISignedUpdate update) => TryPeek(update.Sender);
        /// <summary>
        /// Tries to get the latest page that sender has opened without removing it from the navigation history.
        /// </summary>
        /// <param name="sender">A certain sender.</param>
        /// <returns>The latest opened page or <see cref="" langword="null"/> if it doesn't exist.</returns>
        public IBotPage? TryPeek(IBotUser sender) => TryPeek(sender.TelegramId);
        public IBotPage? TryPeek(long senderId)
        {
            if (!Navigations.ContainsKey(senderId)) return null;
            if (Navigations[senderId].TryPeek(out IBotPage? res)) return res;
            return null;
        }
        /// <summary>
        /// Gets the latest page that update's sender has opened, removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/>
        /// for <paramref name="senderId"/> doesn't exist.</exception>
        public IBotPage Pop(ISignedUpdate update) => Pop(update.Sender);
        /// <summary>
        /// Gets the latest page that sender has opened, removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="sender">A certain sender.</param>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/>
        /// for <paramref name="senderId"/> doesn't exist.</exception>
        public IBotPage Pop(IBotUser sender) => Pop(sender.TelegramId);
        /// <summary>
        /// Gets the latest page that sender has opened, removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="senderId">User's id.</param>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/>
        /// for <paramref name="senderId"/> doesn't exist.</exception>
        public IBotPage Pop(long senderId) => TryPop(senderId)
            ?? throw new NotDefinedException(GetType(), typeof(PageSessionData), senderId.ToString());
        /// <summary>
        /// Tries to get the latest page that update's sender has opened, removing it from the navigation history.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>The latest opened page or <see cref="" langword="null"/> if it doesn't exist.</returns>
        public IBotPage? TryPop(ISignedUpdate update) => TryPop(update.Sender);
        /// <summary>
        /// Tries to get the latest page that sender has opened, removing it from the navigation history.
        /// </summary>
        /// <param name="sender">A certain sender.</param>
        /// <returns>The latest opened page or <see cref="" langword="null"/> if it doesn't exist.</returns>
        public IBotPage? TryPop(IBotUser sender) => TryPop(sender.TelegramId);
        public IBotPage? TryPop(long senderId)
        {
            if (!Navigations.ContainsKey(senderId)) return null;
            if (Navigations[senderId].TryPop(out IBotPage? res)) return res;
            return null;
        }
        #endregion

        /// <summary>
        /// An internal storage that provides access to all saved pages data.
        /// </summary>
        private List<IBotPage> DefinedPages { get; } = new();
        /// <summary>
        /// Determines whether a page is defined. Uses <see cref="IBotPage.PageId"/>.
        /// </summary>
        /// <param name="page">A page to lookup.</param>
        /// <returns><see langword="true"/> if page exists. Otherwise <see langword="false"/>.</returns>
        public bool IsDefined(IBotPage page) => TryGetDefined(page) is not null;
        public bool IsDefined(string pageId) => TryGetDefined(pageId) is not null;
        /// <summary>
        /// Saves a new page data into the internal storage, that can be accessed
        /// via <see cref="GetDefined(string)"/> method in future.
        /// </summary>
        /// <param name="item">Page to define by saving it into the internal storage.</param>
        /// <exception cref="NullIdException">Thrown when id of an <paramref name="item"/> is null or empty.</exception>
        /// <exception cref="DuplicationException">Thrown when an item with same id already exists.</exception>
        public void Define(IBotPage item)
        {
            if (string.IsNullOrEmpty(item.PageId)) throw new NullIdException(GetType(), item.GetType());
            if (IsDefined(item)) throw new DuplicationException(GetType(), item.GetType(), item.PageId);
            DefinedPages.Add(item);
        }
        /// <summary>
        /// Gets defined via <see cref="Define(IBotPage)"/> page from the internal storage by <paramref name="page"/>'s id.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="page">Page to recover (by its id).</param>
        /// <returns>Defined page.</returns>
        /// <exception cref="NotDefinedException">Thrown when a page with a specified id doesn't exist.</exception>
        public IBotPage GetDefined(IBotPage page) => GetDefined(page.PageId);
        /// <summary>
        /// Gets defined via <see cref="Define(IBotPage)"/> page from the internal storage by page's id.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <param name="pageId">Page's id.</param>
        /// <returns>Defined page.</returns>
        /// <exception cref="NotDefinedException">Thrown when a page with a specified id doesn't exist.</exception>
        public IBotPage GetDefined(string pageID) => TryGetDefined(pageID) ?? throw new NotDefinedException(GetType(), typeof(IBotPage), pageID);
        /// <summary>
        /// Tries to get defined via <see cref="Define(IBotPage)"/> page from the internal storage by <paramref name="page"/>'s id.
        /// </summary>
        /// <param name="page">Page to recover (by its id).</param>
        /// <returns>Defined page or <see langword="null"/> if doesn't exist.</returns>
        public IBotPage? TryGetDefined(IBotPage page) => TryGetDefined(page.PageId);
        public IBotPage? TryGetDefined(string pageID) => DefinedPages.Find(x => x.PageId == pageID);

        public IBotAction<SignedCallbackUpdate> BackCallabck => new DefaultCallback(IMenuManager.BackCallBase, "{back menu}", BackMenuAsync);
        private async Task BackMenuAsync(SignedCallbackUpdate update)
        {
            // Sesion invalid => Handle Expired
            if (!CheckSession(update))
            {
                await HandleSessionExpiredAsync(update);
                return;
            }
            // Attempt to delete current page.
            // if null => Handle Expired
            if (TryPop(update) is null)
            {
                await HandleSessionExpiredAsync(update);
                return;
            }
            // Attempt to extract previous page from a history.
            // if null (nothing to open) => Handle Expired
            IBotPage? page = TryPop(update);
            if (page is null)
            {
                await HandleSessionExpiredAsync(update);
                return;
            }
            // Push and open gotten previous page.
            await PushPageAsync(page, update);
        }

        public IArgedAction<NavigationArgs, SignedCallbackUpdate> OpenPageCallabck => new BotArgedCallback<NavigationArgs>(IMenuManager.OpenCallBase, "{open menu}", OpenMenuAsync);
        private async Task OpenMenuAsync(NavigationArgs args, SignedCallbackUpdate update)
            => await (CheckSession(update) ? PushPageAsync(args.Page, update, args.Refresh) : HandleSessionExpiredAsync(update));

        public async Task PushPageAsync(IBotPage page, ISignedUpdate update, bool refresh = false)
        {
            if (refresh && CheckSession(update))
                Navigations.Remove(update.Sender.TelegramId);

            var previous = TryPeek(update);
            IOutputMessage displayMes = page.BuildMessage(previous, update);

            // M.Update
            int mesId;
            if (update is SignedCallbackUpdate callback)
            {
                mesId = callback.TriggerMessageId;
                await Owner.DeliveryService
                    .ReplyToSender(new EditWrapper(displayMes, mesId), callback);
            }
            // M.Push
            // [!] TODO : should be updated after .AdvancedMessaging
            else
            {
                var response = await Owner.DeliveryService.ReplyToSender(displayMes, update);
                mesId = response.Message is not null
                    ? response.Message.MessageId
                    : throw new Exception($"{nameof(DefaultMenuManager)},Push Page => null message returned.");

            }
            // Gets session or generates a new one and pushes page to it.
            Push(page, GetSession(update, mesId));
        }

        /// <summary>
        /// Internal use only. Realization of <see cref="OnCompilation"/>.
        /// <para>Sets up converting rules.</para>
        /// </summary>
        private void OnReflectiveCompile(object sender, BotManager owner)
        {
            var pageRule = new ConvertRule<IBotPage>(pid => IsDefined(pid) ? ConvertResult<IBotPage>.OK(TryGetDefined(pid)!) : ConvertResult<IBotPage>.Incorrect($"Page with id {pid} is not defined."));
            owner.ResolveService<IArgsSerilalizerService>().AddRule(pageRule);
        }
        public void ApplyFor(IActionManager<SignedCallbackUpdate> callbackManager)
        {
            callbackManager.AddSafely(OpenPageCallabck);
            callbackManager.AddSafely(BackCallabck);
        }

        /// <summary>
        /// Handles an error when <see cref="PageSessionData"/> stored in <see cref="Navigations"/> doesn't exist
        /// or throws unpredicted exception. Blocks <paramref name="update"/>'s message menu page
        /// by removing its inline menu and notifying sender that session has been expired.
        /// <para>Notification content can be overriden via <see cref="BotManager.Localizator"/>
        /// with <see cref="SessionExpiredKey"/> local key.</para>
        /// </summary>
        /// <param name="update">An update that has raised an exception.</param>
        private static async Task HandleSessionExpiredAsync(SignedCallbackUpdate update)
        {
            var mes = new OutputMessageText(update.Owner.ResolveBotString(SessionExpiredKey))
            {
                Menu = null
            };
            await update.Owner.DeliveryService.ReplyToSender(new EditWrapper(mes, update.TriggerMessageId), update);
        }

        public override string ToString() => DebugName ?? nameof(DefaultMenuManager);
    }
}