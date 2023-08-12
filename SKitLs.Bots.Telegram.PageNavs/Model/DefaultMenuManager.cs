using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Args;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using SKitLs.Bots.Telegram.PageNavs.resources.settings;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// Default implementation of the <see cref="IMenuManager"/> interface, providing methods for inline message navigation.
    /// Utilizes <see cref="PageSessionData"/> to provide a simple navigation data stack-storage for a single message.
    /// Enables two methods of navigation: forward by opening a new page via <see cref="OpenPageCallback"/>
    /// and backward by rolling back to a previous page via <see cref="BackCallback"/>.
    /// <para>
    /// Allows storage of menu pages' data, providing access to it, and handling special menu updates released via callbacks.
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
        /// <inheritdoc/>
        public string? DebugName { get; set; }

        private BotManager? _owner;
        /// <inheritdoc/>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <inheritdoc/>
        public Action<object, BotManager>? OnCompilation => OnReflectiveCompile;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultMenuManager"/> with optional debug name.
        /// </summary>
        /// <param name="debugName">Optional debug name.</param>
        public DefaultMenuManager(string? debugName = null) => DebugName = debugName;

        #region Sessions and navigation data.
        /// <summary>
        /// An internal storage that provides access to all saved navigation sessions.
        /// </summary>
        private Dictionary<long, PageSessionData> Navigations { get; } = new();
        
        /// <inheritdoc cref="CheckSession(SignedCallbackUpdate)"/>
        private bool CheckSession(ISignedUpdate update) => update is SignedCallbackUpdate callback && CheckSession(callback);
        /// <inheritdoc cref="CheckSession(long, int)"/>
        /// <param name="update">An incoming update.</param>
        private bool CheckSession(SignedCallbackUpdate update) => CheckSession(update.Sender.TelegramId, update.TriggerMessageId);
        /// <summary>
        /// Detects whether a session for the specified data exists and valid.
        /// </summary>
        /// <param name="userId">The user's ID to check its session.</param>
        /// <param name="messageId">The message's ID to check session for.</param>
        /// <returns><see langword="true"/> if session exists and its <see cref="PageSessionData.MessageId"/>
        /// is equal to the <paramref name="messageId"/>;
        /// otherwise <see langword="false"/>.</returns>
        private bool CheckSession(long userId, int messageId) => Navigations.ContainsKey(userId) && Navigations[userId].MessageId == messageId;

        /// <inheritdoc cref="GetSession(long, int)"/>
        /// <param name="update">An incoming signed update.</param>
        /// <param name="messageId">The message's ID.</param>
        public PageSessionData GetSession(ISignedUpdate update, int messageId) => GetSession(update.Sender, messageId);
        /// <inheritdoc cref="GetSession(long, int)"/>
        /// <param name="sender">A certain sender.</param>
        /// <param name="messageId">The message's ID.</param>
        public PageSessionData GetSession(IBotUser sender, int messageId) => GetSession(sender.TelegramId, messageId);
        /// <summary>
        /// Gets the user's menu session by their ID for a certain message.
        /// If it doesn't exist, generates and returns a new one.
        /// </summary>
        /// <param name="senderId">User's ID.</param>
        /// <param name="messageId">Message's ID.</param>
        /// <returns>An existing or a new <see cref="PageSessionData"/> for a certain user and message.</returns>
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

        #region Sessions manipulation
        /// <inheritdoc cref="Push(IBotPage, long, int)"/>
        /// <param name="page">A page to push.</param>
        /// <param name="session">The session to push page to.</param>
        public void Push(IBotPage page, PageSessionData session) => Push(page, session.OwnerId, session.MessageId);
        /// <inheritdoc cref="Push(IBotPage, IBotUser, int)"/>
        /// <param name="page">A page to push.</param>
        /// <param name="update">An incoming update.</param>
        /// <param name="messageId">A message that host menu id.</param>
        public void Push(IBotPage page, ISignedUpdate update, int messageId) => Push(page, update.Sender, messageId);
        /// <inheritdoc cref="Push(IBotPage, long, int)"/>
        /// <param name="page">A page to push.</param>
        /// <param name="sender">A certain sender.</param>
        /// <param name="messageId">A message that host menu id.</param>
        public void Push(IBotPage page, IBotUser sender, int messageId) => Push(page, sender.TelegramId, messageId);
        /// <inheritdoc/>
        public void Push(IBotPage page, long senderId, int messageId) => GetSession(senderId, messageId).Push(page);

        /// <inheritdoc cref="Peek(IBotUser)"/>
        /// <param name="update">An incoming update.</param>
        public IBotPage Peek(ISignedUpdate update) => Peek(update.Sender);
        /// <inheritdoc cref="Peek(long)"/>
        /// <param name="sender">A certain sender.</param>
        public IBotPage Peek(IBotUser sender) => Peek(sender.TelegramId);
        /// <inheritdoc/>
        /// <remarks>Throws an exception if that page does not exist.</remarks>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/>
        /// for <paramref name="senderId"/> doesn't exist.</exception>
        public IBotPage Peek(long senderId) => TryPeek(senderId) ?? throw new NotDefinedException(this, typeof(PageSessionData), senderId.ToString());

        /// <inheritdoc cref="TryPeek(IBotUser)"/>
        /// <param name="update">An incoming update.</param>
        public IBotPage? TryPeek(ISignedUpdate update) => TryPeek(update.Sender);
        /// <inheritdoc cref="TryPeek(long)"/>
        /// <param name="sender">A certain sender.</param>
        public IBotPage? TryPeek(IBotUser sender) => TryPeek(sender.TelegramId);
        /// <inheritdoc/>
        public IBotPage? TryPeek(long senderId)
        {
            if (!Navigations.ContainsKey(senderId)) return null;
            if (Navigations[senderId].TryPeek(out IBotPage? res)) return res;
            return null;
        }

        /// <inheritdoc cref="Pop(ISignedUpdate)"/>
        /// <param name="update">An incoming update.</param>
        public IBotPage Pop(ISignedUpdate update) => Pop(update.Sender);
        /// <inheritdoc cref="Pop(long)"/>
        /// <param name="sender">A certain sender.</param>
        public IBotPage Pop(IBotUser sender) => Pop(sender.TelegramId);
        /// <inheritdoc/>
        /// <remarks>Throws an exception if that page doesn't exist.</remarks>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/>
        /// for <paramref name="senderId"/> doesn't exist.</exception>
        public IBotPage Pop(long senderId) => TryPop(senderId) ?? throw new NotDefinedException(this, typeof(PageSessionData), senderId.ToString());

        /// <inheritdoc cref="TryPop(IBotUser)"/>
        /// <param name="update">An incoming update.</param>
        public IBotPage? TryPop(ISignedUpdate update) => TryPop(update.Sender);
        /// <inheritdoc cref="TryPop(long)"/>
        /// <param name="sender">A certain sender.</param>
        public IBotPage? TryPop(IBotUser sender) => TryPop(sender.TelegramId);
        /// <inheritdoc/>
        public IBotPage? TryPop(long senderId)
        {
            if (!Navigations.ContainsKey(senderId)) return null;
            if (Navigations[senderId].TryPop(out IBotPage? res)) return res;
            return null;
        }
        #endregion

        #region Page Definition
        /// <summary>
        /// An internal storage that provides access to all saved pages data.
        /// </summary>
        private List<IBotPage> DefinedPages { get; } = new();

        /// <inheritdoc cref="IsDefined(string)"/>
        /// <param name="page">A page to lookup.</param>
        public bool IsDefined(IBotPage page) => TryGetDefined(page) is not null;
        /// <inheritdoc/>
        public bool IsDefined(string pageId) => TryGetDefined(pageId) is not null;

        /// <inheritdoc/>
        /// <exception cref="NullIdException">Thrown when id of an <paramref name="item"/> is null or empty.</exception>
        /// <exception cref="DuplicationException">Thrown when an item with same id already exists.</exception>
        public void Define(IBotPage item)
        {
            if (string.IsNullOrEmpty(item.PageId)) throw new NullIdException(GetType(), item.GetType());
            if (IsDefined(item)) throw new DuplicationException(GetType(), item.GetType(), item.PageId);
            DefinedPages.Add(item);
        }

        /// <inheritdoc cref="GetDefined(string)"/>
        /// <param name="page">Page to recover (by its id).</param>
        public IBotPage GetDefined(IBotPage page) => GetDefined(page.PageId);
        /// <inheritdoc/>
        /// <remarks>Throws an exception if that page does not exist.</remarks>
        /// <exception cref="NotDefinedException">Thrown when a page with a specified id doesn't exist.</exception>
        public IBotPage GetDefined(string pageId) => TryGetDefined(pageId) ?? throw new NotDefinedException(this, typeof(IBotPage), pageId);

        /// <inheritdoc cref="TryGetDefined(string)"/>
        /// <param name="page">Page to recover (by its id).</param>\
        public IBotPage? TryGetDefined(IBotPage page) => TryGetDefined(page.PageId);
        /// <inheritdoc/>
        public IBotPage? TryGetDefined(string pageId) => DefinedPages.Find(x => x.PageId == pageId);
        #endregion

        /// <inheritdoc/>
        public IBotAction<SignedCallbackUpdate> BackCallback => new DefaultCallback(PNSettings.BackCallBase, "{back menu}", BackMenuAsync);
        private async Task BackMenuAsync(SignedCallbackUpdate update)
        {
            // Session invalid => Handle Expired
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

        /// <inheritdoc/>
        public IArgedAction<NavigationArgs, SignedCallbackUpdate> OpenPageCallback => new BotArgedCallback<NavigationArgs>(PNSettings.OpenCallBase, "{open menu}", OpenMenuAsync);
        private async Task OpenMenuAsync(NavigationArgs args, SignedCallbackUpdate update)
            => await (CheckSession(update) ? PushPageAsync(args.Page, update, args.Refresh) : HandleSessionExpiredAsync(update));

        /// <inheritdoc/>
        public async Task PushPageAsync(IBotPage page, ISignedUpdate update, bool refresh = false)
        {
            if (refresh && Navigations.ContainsKey(update.Sender.TelegramId))
                Navigations.Remove(update.Sender.TelegramId);

            var previous = TryPeek(update);
            var displayMes = await (await page.BuildMessageAsync(previous, update)).BuildContentAsync(update);

            // M.Update
            int mesId;
            if (update is SignedCallbackUpdate callback)
            {
                mesId = callback.TriggerMessageId;
                await Owner.DeliveryService.AnswerSenderAsync(new EditWrapper(displayMes, mesId), callback);
            }
            // M.Push
            else
            {
                var response = await Owner.DeliveryService.AnswerSenderAsync(displayMes, update);
                mesId = response.SentMessage.MessageId;
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
            var pageRule = new ConvertRule<IBotPage>(pid => IsDefined(pid)
                ? ConvertResult<IBotPage>.OK(TryGetDefined(pid)!)
                : ConvertResult<IBotPage>.Incorrect($"Page with id {pid} is not defined."));
            owner.ResolveService<IArgsSerializeService>().AddRule(pageRule);
        }
        /// <summary>
        /// Applies inherited class to <paramref name="callbackManager"/>, defining and integrating necessary data to it.
        /// <para>
        /// For example, if <see cref="IApplicant{T}"/> is applicable to <see cref="IActionManager{TUpdate}"/>
        /// use <see cref="IActionManager{TUpdate}.AddSafely(IBotAction{TUpdate})"/> to integrate all <see cref="IBotAction{TUpdate}"/>,
        /// defined in applicant.
        /// </para>
        /// </summary>
        /// <param name="callbackManager">An instance that this class should be applied to.</param>
        public void ApplyTo(IActionManager<SignedCallbackUpdate> callbackManager)
        {
            callbackManager.AddSafely(OpenPageCallback);
            callbackManager.AddSafely(BackCallback);
        }

        /// <summary>
        /// Handles an error when <see cref="PageSessionData"/> stored in <see cref="Navigations"/> doesn't exist
        /// or throws unpredicted exception. Blocks <paramref name="update"/>'s message menu page
        /// by removing its inline menu and notifying sender that session has been expired.
        /// <para>Notification content can be overridden via <see cref="BotManager.Localizator"/>
        /// with <see cref="PNSettings.SessionExpiredLocalKey"/> local key.</para>
        /// </summary>
        /// <param name="update">An update that has raised an exception.</param>
        private static async Task HandleSessionExpiredAsync(SignedCallbackUpdate update)
        {
            var mes = new TelegramTextMessage(update.Owner.ResolveBotString(PNSettings.SessionExpiredLocalKey));
            await update.Owner.DeliveryService.AnswerSenderAsync(new EditWrapper(mes, update.TriggerMessageId), update);
        }

        /// <inheritdoc/>
        public override string ToString() => DebugName ?? nameof(DefaultMenuManager);
    }
}