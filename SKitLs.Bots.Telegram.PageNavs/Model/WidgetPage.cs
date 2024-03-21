using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// <see cref="WidgetPage"/> class implements the <see cref="IBotPage"/> interface and represents a dynamic page in the bot.
    /// It allows to generate a page with a customizable label and body based on incoming updates.
    /// </summary>
    public sealed class WidgetPage : IBotPage
    {
        /// <inheritdoc/>
        public event Func<ISignedUpdate, Task>? PageOpened;

        /// <inheritdoc/>
        public string PageId { get; private init; }

        /// <summary>
        /// A method that generates dynamic label for a page link.
        /// </summary>
        public Func<ISignedUpdate, string> LabelBuilder { get; private init; }

        /// <summary>
        /// Page body that would appeal as a page representation. Can be customized based on incoming update.
        /// </summary>
        public IOutputMessage PageView { get; private init; }

        /// <inheritdoc/>
        public IPageMenu Menu { get; set; }

        /// <summary>
        /// Initializes a new instance of a <see cref="WidgetPage"/> class with specified data.
        /// </summary>
        /// <param name="pageId">Page's unique identifier.</param>
        /// <param name="label">Page's display label.</param>
        /// <param name="pageView">Page's message body. Does not copy any actions from its <see cref="IOutputMessage.Menu"/>.</param>
        /// <param name="menu">Page's menu. If <paramref name="menu"/> is <see langword="null"/>,
        /// <see cref="Menu"/> will be set to default <see cref="PageNavMenu"/>.</param>
        public WidgetPage(string pageId, string label, IOutputMessage pageView, IPageMenu? menu = null) : this(pageId, u => label, pageView, menu) { }

        /// <summary>
        /// Initializes a new instance of a <see cref="WidgetPage"/> class with specified data.
        /// </summary>
        /// <param name="pageId">Page's unique identifier.</param>
        /// <param name="labelBuilder">Page's display label builder.</param>
        /// <param name="pageView">Page's message body. Does not copy any actions from its <see cref="IOutputMessage.Menu"/>.</param>
        /// <param name="menu">Page's menu. If <paramref name="menu"/> is <see langword="null"/>,
        /// <see cref="Menu"/> will be set to default <see cref="PageNavMenu"/>.</param>
        public WidgetPage(string pageId, Func<ISignedUpdate, string> labelBuilder, IOutputMessage pageView, IPageMenu? menu = null)
        {
            PageId = pageId;
            LabelBuilder = labelBuilder;
            PageView = pageView;
            Menu = menu ?? new PageNavMenu();
        }

        /// <inheritdoc/>
        public string GetLabel(ISignedUpdate update) => LabelBuilder(update);

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
            var clone = (IOutputMessage)PageView.Clone();
            clone.Menu = await Menu.BuildAsync(previous, this, update);
            return clone;
        }

        /// <inheritdoc/>
        public string GetPacked() => PageId;

        /// <inheritdoc/>
        public override string ToString() => PageId;
    }
}