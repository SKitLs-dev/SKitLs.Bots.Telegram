using SKitLs.Bots.Telegram.AdvancedMessages.Messages;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Pages.Menus;

namespace SKitLs.Bots.Telegram.PageNavs.Pages
{
    /// <summary>
    /// Represents a widget page in the bot.
    /// </summary>
    public class WidgetPage : WidgetPageBase
    {
        /// <summary>
        /// Gets or sets the task for building the label of the page dynamically.
        /// </summary>
        public DynamicPageStringTask LabelBuilder { get; set; }

        /// <summary>
        /// Gets or sets the task for building the body of the page dynamically.
        /// </summary>
        public DynamicPageMessageTask BodyBuilder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetPage"/> class with specified data.
        /// </summary>
        /// <param name="pageId">The unique identifier of the page.</param>
        /// <param name="labelBuilder">The task for building the label of the page dynamically.</param>
        /// <param name="bodyBuilder">The task for building the body of the page dynamically.</param>
        /// <param name="menu">The menu of the page. If null, the default menu will be used.</param>
        public WidgetPage(string pageId, DynamicPageStringTask labelBuilder, DynamicPageMessageTask bodyBuilder, IPageMenu? menu = null)
            : base(pageId, menu)
        {
            LabelBuilder = labelBuilder;
            BodyBuilder = bodyBuilder;
        }

        /// <inheritdoc/>
        public override async Task<string> BuildLabelAsync(ISignedUpdate update) => await LabelBuilder(this, update);

        /// <inheritdoc/>
        public override async Task<IOutputMessage> BuildOutputMessage(ISignedUpdate update) => await BodyBuilder(this, update);
    }
}