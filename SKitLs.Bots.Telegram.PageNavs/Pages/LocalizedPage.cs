using SKitLs.Bots.Telegram.AdvancedMessages.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Messages.Text;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Pages.Menus;

namespace SKitLs.Bots.Telegram.PageNavs.Pages
{
    /// <summary>
    /// Represents a localized page with text content.
    /// </summary>
    public class LocalizedPage : WidgetPageBase
    {
        /// <summary>
        /// Gets or sets the format mask for the label key.
        /// </summary>
        public static string LabelKeyMask { get; set; } = "{0}";

        /// <summary>
        /// Gets or sets the format mask for the page key.
        /// </summary>
        public static string PageKeyMask { get; set; } = "{0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedPage"/> class with the specified parameters.
        /// </summary>
        /// <param name="pageId">The unique identifier of the page.</param>
        /// <param name="menu">The menu associated with the page.</param>
        public LocalizedPage(string pageId, IPageMenu? menu = null) : base(pageId, menu)
        { }

        /// <summary>
        /// Resolves the parameters for the label format dynamically.
        /// </summary>
        public virtual async Task<string[]> ResolveLabelFormatParameters(ISignedUpdate update) => await Task.FromResult(Array.Empty<string>());

        /// <summary>
        /// Resolves the parameters for the body format dynamically.
        /// </summary>
        public virtual async Task<string[]> ResolveBodyFormatParameters(ISignedUpdate update) => await Task.FromResult(Array.Empty<string>());

        /// <inheritdoc/>
        public override async Task<string> BuildLabelAsync(ISignedUpdate update)
        {
            var format = await ResolveLabelFormatParameters(update);
            var key = string.Format(LabelKeyMask, PageId);
            return update?.Owner.ResolveBotString(key, format) ?? key;
        }

        /// <inheritdoc/>
        public override async Task<IOutputMessage> BuildOutputMessage(ISignedUpdate update)
        {
            var format = await ResolveBodyFormatParameters(update);
            var key = string.Format(PageKeyMask, PageId);
            return new OutputMessageText(update?.Owner.ResolveBotString(key, format) ?? key);
        }
    }
}