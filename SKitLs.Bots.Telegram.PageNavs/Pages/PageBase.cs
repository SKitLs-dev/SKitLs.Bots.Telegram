using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Pages;
using SKitLs.Bots.Telegram.PageNavs.Pages.Menus;

namespace SKitLs.Bots.Telegram.PageNavs.Model.Pages
{
    /// <summary>
    /// Represents a base class for pages in the navigation system.
    /// </summary>
    public abstract class PageBase : IBotPage
    {
        /// <inheritdoc/>
        public event Func<ISignedUpdate, Task>? PageOpened;

        /// <inheritdoc/>
        public virtual string PageId { get; private init; }

        /// <inheritdoc/>
        public virtual IPageMenu Menu { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageBase"/> class with the specified parameters.
        /// </summary>
        /// <param name="pageId">The unique identifier of the page.</param>
        /// <param name="menu">The menu associated with the page.</param>
        public PageBase(string pageId, IPageMenu menu)
        {
            PageId = pageId ?? throw new ArgumentNullException(nameof(pageId));
            Menu = menu ?? throw new ArgumentNullException(nameof(menu));
        }

        /// <inheritdoc/>
        public abstract Task<string> BuildLabelAsync(ISignedUpdate update);

        /// <inheritdoc/>
        public abstract Task<ITelegramMessage> BuildMessageAsync(IBotPage? previous, ISignedUpdate update);

        /// <inheritdoc/>
        public virtual async Task NotifyPageOpenedAsync(ISignedUpdate update)
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
        public virtual string GetPacked() => PageId;

        /// <inheritdoc/>
        public override string ToString() => PageId;
    }
}