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
        /// <summary>
        /// An unique page's identifier.
        /// </summary>
        public string PageId { get; private init; }

        /// <summary>
        /// A method that generates dynamic label for a page link.
        /// </summary>
        public Func<ISignedUpdate, string> LabelBuilder { get; private init; }

        /// <summary>
        /// Dynamic page body that would appeal as a page representation. Can be customized based on incoming update.
        /// </summary>
        public IDynamicMessage PageView { get; private init; }

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

        /// <summary>
        /// Returns a string that should be printed on the inline keyboard menu as a navigation label.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>A string that represents an instance as a navigation label.</returns>
        public string GetLabel(ISignedUpdate update) => LabelBuilder(update);

        /// <summary>
        /// Converts page data to a printable <see cref="IOutputMessage"/> that should be printed
        /// based on incoming <paramref name="update"/>.
        /// Optionally can add "Back" Button if <paramref name="previous"/> argument is not <see langword="null"/>.
        /// </summary>
        /// <param name="previous">A page to which should lead "Back" Button.</param>
        /// <param name="update">An incoming update.</param>
        /// <returns>Built ready-to-print message.</returns>
        public IOutputMessage BuildMessage(IBotPage? previous, ISignedUpdate update)
        {
            var mes = PageView.BuildWith(update);
            mes.Menu = Menu.Build(previous, this, update);
            return mes;
        }

        /// <summary>
        /// Generates a string that could be used as a representation of this object.
        /// </summary>
        /// <returns>A string that could be used as a representation of this object.</returns>
        public string GetPacked() => PageId;
        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override string ToString() => PageId;
    }
}