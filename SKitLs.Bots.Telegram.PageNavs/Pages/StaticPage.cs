using SKitLs.Bots.Telegram.AdvancedMessages.Messages;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model.Pages;
using SKitLs.Bots.Telegram.PageNavs.Pages.Menus;

namespace SKitLs.Bots.Telegram.PageNavs.Pages
{
    /// <summary>
    /// Represents a static page in the bot.
    /// </summary>
    public class StaticPage : PageBase
    {
        /// <summary>
        /// Gets the dynamic label for the page link.
        /// </summary>
        public virtual string Label { get; private init; }

        /// <summary>
        /// Gets the message body of the page.
        /// </summary>
        public virtual IOutputMessage PageView { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticPage"/> class with specified data.
        /// </summary>
        /// <param name="pageId">The unique identifier of the page.</param>
        /// <param name="label">The display label of the page.</param>
        /// <param name="pageView">The message body of the page. Does not copy any actions from its menu.</param>
        /// <param name="menu">The menu of the page. If null, the default menu will be used.</param>
        public StaticPage(string pageId, string label, IOutputMessage pageView, IPageMenu? menu = null) : base(pageId, menu ?? new PageNavMenu())
        {
            Label = label;
            PageView = pageView;
        }

        /// <inheritdoc/>
        public override async Task<string> BuildLabelAsync(ISignedUpdate update) => await Task.FromResult(Label);

        /// <inheritdoc/>
        public override async Task<ITelegramMessage> BuildMessageAsync(IBotPage? previous, ISignedUpdate update)
        {
            var clone = (IOutputMessage)PageView.Clone();
            clone.Menu = await Menu.BuildAsync(previous, this, update);
            return await clone.BuildContentAsync(update);
        }
    }
}