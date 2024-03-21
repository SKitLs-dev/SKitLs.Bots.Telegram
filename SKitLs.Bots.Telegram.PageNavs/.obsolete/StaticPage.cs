using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// <see cref="StaticPage"/> class implements the <see cref="IBotPage"/> interface and represents a static menu page in the bot.
    /// It holds and displays preset text content, providing a fixed and unchanging page representation.
    /// </summary>
    [Obsolete("Due to .Core [v2.1] all the pages are dynamic now. Use WidgetPage instead.", true)]
    public sealed class StaticPage : IBotPage
    {
        /// <inheritdoc/>
        public event Func<ISignedUpdate, Task>? PageOpened;

        /// <inheritdoc/>
        public string PageId { get; private init; }

        /// <summary>
        /// A string that represents <see cref="StaticPage"/> on the navigation menu bar.
        /// </summary>
        public string Label { get; private init; }

        /// <inheritdoc/>
        public string GetLabel(ISignedUpdate update) => Label;

        /// <summary>
        /// Static preset page body that would appeal as a page representation.
        /// </summary>
        public IOutputMessage PageView { get; private init; }

        /// <inheritdoc/>
        public IPageMenu Menu { get; set; }

        /// <summary>
        /// Initializes a new instance of a <see cref="StaticPage"/> with specified data.
        /// Uses default <see cref="OutputMessageText"/> as a <see cref="PageView"/>.
        /// </summary>
        /// <param name="pageId">Page's unique identifier.</param>
        /// <param name="label">Page's display label.</param>
        /// <param name="pageViewText">Page's message text.</param>
        /// <param name="menu">Page's menu. If <paramref name="menu"/> is <see langword="null"/>,
        /// <see cref="Menu"/> will be set to default <see cref="PageNavMenu"/>.</param>
        public StaticPage(string pageId, string label, string pageViewText, IPageMenu? menu = null) : this(pageId, label, new OutputMessageText(pageViewText), menu) { }
        /// <summary>
        /// Initializes a new instance of a <see cref="StaticPage"/> with specified data.
        /// </summary>
        /// <param name="pageId">Page's unique identifier.</param>
        /// <param name="label">Page's display label.</param>
        /// <param name="pageView">Page's message body. Does not copy any actions from its <see cref="IOutputMessage.Menu"/>.</param>
        /// <param name="menu">Page's menu. If <paramref name="menu"/> is <see langword="null"/>,
        /// <see cref="Menu"/> will be set to default <see cref="PageNavMenu"/>.</param>
        public StaticPage(string pageId, string label, IOutputMessage pageView, IPageMenu? menu = null)
        {
            PageId = pageId;
            Label = label;
            PageView = pageView;
            Menu = menu ?? new PageNavMenu();
        }

        /// <inheritdoc/>
        public async Task NotifyPageOpenedAsync(ISignedUpdate update)
        {
            if (PageOpened is not null)
            {
                foreach (var handler in PageOpened.GetInvocationList())
                {
                    if (handler is Func<ISignedUpdate, Task> asyncHandler)
                    {
                        await asyncHandler(update);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<IOutputMessage> BuildMessageAsync(IBotPage? previous, ISignedUpdate update)
        {
            var mes = (IOutputMessage)PageView.Clone();
            mes.Menu = await Menu.BuildAsync(previous, this, update);
            return mes;
        }

        /// <inheritdoc/>
        public string GetPacked() => PageId;
        /// <inheritdoc/>
        public override string ToString() => PageId;
    }
}