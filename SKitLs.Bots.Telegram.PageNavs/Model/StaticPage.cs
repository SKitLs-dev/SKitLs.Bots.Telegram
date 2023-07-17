using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// The representation of a static menu page. Holds and displays preset text.
    /// </summary>
    public class StaticPage : IBotPage
    {
        public string PageId { get; private set; }
        /// <summary>
        /// A string that represents <see cref="StaticPage"/> on the navigation manu bar.
        /// </summary>
        public string Label { get; private set; }
        public string GetLabel(ISignedUpdate update) => Label;

        /// <summary>
        /// Static preset page body that would appeal as a page representation.
        /// </summary>
        public IOutputMessage PageView { get; private set; }
        /// <summary>
        /// Page's menu. If <see langword="null"/> is set to default <see cref="PageNavMenu"/>.
        /// </summary>
        public IPageMenu Menu { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="StaticPage"/> with specified data.
        /// Uses default <see cref="OutputMessageText"/> as a <see cref="PageView"/>.
        /// </summary>
        /// <param name="pageId">Page's unique identifier.</param>
        /// <param name="label">Page's display label.</param>
        /// <param name="pageViewText">Page's message text.</param>
        /// <param name="menu">Page's menu. If <paramref name="menu"/> is <see langword="null"/>,
        /// <see cref="Menu"/> will be set to default <see cref="PageNavMenu"/>.</param>
        public StaticPage(string pageId, string label, string pageViewText, IPageMenu? menu = null)
            : this(pageId, label, new OutputMessageText(pageViewText), menu) { }
        /// <summary>
        /// Creates a new instance of a <see cref="StaticPage"/> with specified data.
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
        public IOutputMessage BuildMessage(IBotPage? previous, ISignedUpdate update)
        {
            var mes = (IOutputMessage)PageView.Clone();
            mes.Menu = Menu.Build(previous, this, update);
            return mes;
        }

        public string GetPacked() => PageId;
        public override string ToString() => PageId;
    }
}