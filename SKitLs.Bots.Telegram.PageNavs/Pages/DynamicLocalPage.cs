using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Pages.Menus;

namespace SKitLs.Bots.Telegram.PageNavs.Pages
{
    /// <summary>
    /// Represents a dynamic page with localized content and parameters that can be collected dynamically.
    /// </summary>
    public class DynamicLocalPage : LocalizedPage
    {
        /// <summary>
        /// A delegate to collect parameters for the label format dynamically.
        /// </summary>
        public DynamicPageStringsTask? LabelParamsCollector { get; set; }

        /// <summary>
        /// A delegate to collect parameters for the body format dynamically.
        /// </summary>
        public DynamicPageStringsTask? BodyParamsCollector { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPage"/> class with the specified parameters.
        /// </summary>
        /// <param name="pageId">The unique identifier of the page.</param>
        /// <param name="bodyParamsCollector">A delegate to collect parameters for the body format dynamically.</param>
        /// <param name="labelParamsCollector">A delegate to collect parameters for the label format dynamically.</param>
        /// <param name="menu">The menu associated with the page.</param>
        public DynamicLocalPage(string pageId, DynamicPageStringsTask? bodyParamsCollector, DynamicPageStringsTask? labelParamsCollector = null, IPageMenu? menu = null) : base(pageId, menu)
        {
            LabelParamsCollector = labelParamsCollector;
            BodyParamsCollector = bodyParamsCollector;
        }

        /// <inheritdoc/>
        public override async Task<string[]> ResolveLabelFormatParameters(ISignedUpdate update)
        {
            if (LabelParamsCollector is not null)
                return await LabelParamsCollector(this, update);
            return await base.ResolveLabelFormatParameters(update);
        }

        /// <inheritdoc/>
        public override async Task<string[]> ResolveBodyFormatParameters(ISignedUpdate update)
        {
            if (BodyParamsCollector is not null)
                return await BodyParamsCollector(this, update);
            return await base.ResolveBodyFormatParameters(update);
        }
    }
}