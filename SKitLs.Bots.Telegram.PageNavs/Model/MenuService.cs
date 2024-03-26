using SKitLs.Bots.Telegram.AdvancedMessages.Editors;
using SKitLs.Bots.Telegram.AdvancedMessages.Messages.Text;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.Services;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Users;
using SKitLs.Bots.Telegram.PageNavs.Args;
using SKitLs.Bots.Telegram.PageNavs.Callbacks;
using SKitLs.Bots.Telegram.PageNavs.Pages;
using SKitLs.Bots.Telegram.PageNavs.Settings;
using Telegram.Bot.Exceptions;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// Default implementation of the <see cref="IMenuService"/> interface, providing methods for inline message navigation.
    /// Utilizes <see cref="PageSessionData"/> to provide a simple navigation data stack-storage for a single message.
    /// Enables two methods of navigation: forward by opening a new page via <see cref="OpenPageCallback"/>
    /// and backward by rolling back to a previous page via <see cref="BackCallback"/>.
    /// <para/>
    /// Allows storage of menu pages' data, providing access to it, and handling special menu updates released via callbacks.
    /// <para/>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IDebugNamed"/>.
    /// </summary>
    public class MenuService : BotServiceBase, IMenuService
    {
        /// <inheritdoc/>
        public override Action<object, BotManager>? OnCompilation => OnReflectiveCompile;

        /// <inheritdoc/>
        public ISessionsManager SessionsManager { get; private init; }

        /// <inheritdoc/>
        public IBotAction<SignedCallbackUpdate> BackCallback { get; set; } = new DefaultBackCallback();

        /// <inheritdoc/>
        public IArgedAction<NavigationArgs, SignedCallbackUpdate> OpenPageCallback { get; set; } = new DefaultOpenPageCallback();

        /// <summary>
        /// Initializes a new instance of <see cref="MenuService"/> with an optional debug name.
        /// </summary>
        /// <param name="sessionsManager">Optional sessions manager instance.</param>
        /// <param name="debugName">Optional debug name.</param>
        public MenuService(ISessionsManager? sessionsManager = null, string? debugName = null)
        {
            SessionsManager = sessionsManager ?? new SessionsManager();
            DebugName = debugName;
        }

        /// <summary>
        /// An internal storage that provides access to all saved page data.
        /// </summary>
        private List<IBotPage> DefinedPages { get; } = [];

        /// <inheritdoc/>
        public bool IsDefined(string pageId) => TryGetDefined(pageId) is not null;

        /// <inheritdoc/>
        /// <exception cref="NullIdException">Thrown when the ID of an <paramref name="item"/> is null or empty.</exception>
        /// <exception cref="DuplicationException">Thrown when an item with the same ID already exists.</exception>
        public void Define(IBotPage item)
        {
            if (string.IsNullOrEmpty(item.PageId))
                throw new NullIdException(GetType(), item.GetType());
            
            if (IsDefined(item.PageId))
                throw new DuplicationException(GetType(), item.GetType(), item.PageId);
            
            DefinedPages.Add(item);
        }

        /// <inheritdoc/>
        public IBotPage? TryGetDefined(string pageId) => DefinedPages.Find(x => x.PageId == pageId);

        /// <inheritdoc/>
        /// <exception cref="NotDefinedException">Thrown when a page with the specified ID doesn't exist.</exception>
        public IBotPage GetDefined(string pageId) => TryGetDefined(pageId) ?? throw new NotDefinedException(this, typeof(IBotPage), pageId);
        
        /// <inheritdoc/>
        public async Task PushPageAsync(IBotPage page, ISignedUpdate update, bool refresh = false)
        {
            if (refresh)
                SessionsManager.RefreshSession(update.Sender.TelegramId);

            var previous = SessionsManager.TryPeek(update);
            var displayMes = await page.BuildMessageAsync(previous, update);

            // M.Update
            var mesId = -1;
            var updated = false;
            if (update is SignedCallbackUpdate callback)
            {
                mesId = callback.TriggerMessageId;
                try
                {
                    await Owner.DeliveryService.AnswerSenderAsync(displayMes.Edit(mesId), callback);
                    updated = true;
                }
                catch (ApiRequestException)
                {
                    updated = false;
                }
            }

            // M.Push
            if (!updated)
            {
                var response = await Owner.DeliveryService.AnswerSenderAsync(displayMes, update);
                mesId = response.SentMessage.MessageId;
            }

            // Gets session or generates a new one and pushes page to it.
            SessionsManager.Push(page, SessionsManager.GetOrInitSession(update, mesId));
            await page.NotifyPageOpenedAsync(update);
        }

        /// <inheritdoc/>
        private void OnReflectiveCompile(object sender, BotManager owner)
        {
            var pageRule = new ConvertRule<IBotPage>(pid => IsDefined(pid)
                ? ConvertResult<IBotPage>.OK(GetDefined(pid)!)
                : ConvertResult<IBotPage>.Incorrect($"Page with id {pid} is not defined."));
            owner.ResolveService<IArgsSerializeService>().AddRule(pageRule);
        }

        /// <inheritdoc/>
        public void ApplyTo(IActionManager<SignedCallbackUpdate> callbackManager)
        {
            callbackManager.AddSafely(OpenPageCallback);
            callbackManager.AddSafely(BackCallback);
        }

        /// <inheritdoc/>
        public override string ToString() => DebugName ?? nameof(MenuService);
    }
}