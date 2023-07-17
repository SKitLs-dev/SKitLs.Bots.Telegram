using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    public class WidgetPage : IBotPage
    {
        public string PageId { get; private set; }
        /// <summary>
        /// A method that generates dynamic label for a page link.
        /// </summary>
        public Func<ISignedUpdate, string> LabelBuilder { get; private set; }

        /// <summary>
        /// Dynamic page body that would appeal as a page representation. Can be customized based on incoming update.
        /// </summary>
        public IDynamicMessage PageView { get; private set; }
        /// <summary>
        /// Page's menu. If <see langword="null"/> is set to default <see cref="PageNavMenu"/>.
        /// </summary>
        public IPageMenu Menu { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="WidgetPage"/> with specified data.
        /// </summary>
        /// <param name="pageId">Page's unique identifier.</param>
        /// <param name="label">Page's display label.</param>
        /// <param name="pageView">Page's message body. Does not copy any actions from its <see cref="IOutputMessage.Menu"/>.</param>
        /// <param name="menu">Page's menu. If <paramref name="menu"/> is <see langword="null"/>,
        /// <see cref="Menu"/> will be set to default <see cref="PageNavMenu"/>.</param>
        public WidgetPage(string pageId, string label, IDynamicMessage pageView, IPageMenu? menu = null)
            : this(pageId, u => label, pageView, menu) { }
        /// <summary>
        /// Creates a new instance of a <see cref="WidgetPage"/> with specified data.
        /// </summary>
        /// <param name="pageId">Page's unique identifier.</param>
        /// <param name="labelBuilder">Page's display label builder.</param>
        /// <param name="pageView">Page's message body. Does not copy any actions from its <see cref="IOutputMessage.Menu"/>.</param>
        /// <param name="menu">Page's menu. If <paramref name="menu"/> is <see langword="null"/>,
        /// <see cref="Menu"/> will be set to default <see cref="PageNavMenu"/>.</param>
        public WidgetPage(string pageId, Func<ISignedUpdate, string> labelBuilder, IDynamicMessage pageView, IPageMenu? menu = null)
        {
            PageId = pageId;
            LabelBuilder = labelBuilder;
            PageView = pageView;
            Menu = menu ?? new PageNavMenu();
        }

        public string GetLabel(ISignedUpdate update) => LabelBuilder(update);
        public IOutputMessage BuildMessage(IBotPage? previous, ISignedUpdate update)
        {
            var mes = PageView.BuildWith(update);
            mes.Menu = Menu.Build(previous, this, update);
            return mes;
        }

        public string GetPacked() => PageId;
        public override string ToString() => PageId;
    }
}