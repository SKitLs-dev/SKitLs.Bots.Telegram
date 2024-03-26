using SKitLs.Bots.Telegram.AdvancedMessages.Messages;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model.Pages;
using SKitLs.Bots.Telegram.PageNavs.Pages.Menus;

namespace SKitLs.Bots.Telegram.PageNavs.Pages
{
    /// <summary>
    /// Represents a base class for widget pages in the bot.
    /// </summary>
    public abstract class WidgetPageBase : PageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetPageBase"/> class with specified data.
        /// </summary>
        /// <param name="pageId">The unique identifier of the page.</param>
        /// <param name="menu">The menu of the page. If null, the default menu will be used.</param>
        public WidgetPageBase(string pageId, IPageMenu? menu = null) : base(pageId, menu ?? new PageNavMenu())
        { }

        /// <summary>
        /// Builds the output message of the page asynchronously.
        /// </summary>
        /// <param name="update">The update information.</param>
        /// <returns>The output message of the page.</returns>
        public abstract Task<IOutputMessage> BuildOutputMessage(ISignedUpdate update);

        /// <inheritdoc/>
        public override async Task<ITelegramMessage> BuildMessageAsync(IBotPage? previous, ISignedUpdate update)
        {
            var label = await BuildLabelAsync(update);
            var message = await BuildOutputMessage(update);
            var @static = new StaticPage(PageId, label, message, Menu);
            return await @static.BuildMessageAsync(previous, update);
        }
    }
}