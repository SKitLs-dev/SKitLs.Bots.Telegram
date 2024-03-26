using SKitLs.Bots.Telegram.AdvancedMessages.Messages;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model;

namespace SKitLs.Bots.Telegram.PageNavs.Pages
{
    /// <summary>
    /// Represents an interface for a menu page instance that can be managed by an <see cref="IMenuService"/>.
    /// <para>
    /// For default implementations, refer to: <see cref="StaticPage"/>.
    /// </para>
    /// </summary>
    public interface IBotPage : IArgPackable
    {
        /// <summary>
        /// Event that occurs when the page is opened.
        /// </summary>
        public event Func<ISignedUpdate, Task>? PageOpened;

        /// <summary>
        /// Represents the unique identifier of the page.
        /// </summary>
        public string PageId { get; }

        ///// <summary>
        ///// Represents the menu associated with the page.
        ///// </summary>
        //public IPageMenu? Menu { get; }

        /// <summary>
        /// Gets a label that should be displayed on the inline keyboard menu as a navigation option.
        /// </summary>
        /// <param name="update">The incoming update.</param>
        /// <returns>A string representing the navigation label for this page.</returns>
        public Task<string> BuildLabelAsync(ISignedUpdate update);

        /// <summary>
        /// Asynchronously notifies subscribers that the page has been opened.
        /// </summary>
        /// <param name="update">The signed update containing relevant information.</param>
        public Task NotifyPageOpenedAsync(ISignedUpdate update);

        /// <summary>
        /// Asynchronously converts the page data to a printable <see cref="IOutputMessage"/> based on the incoming <paramref name="update"/>.
        /// Optionally, a "Back" button can be added if the <paramref name="previous"/> argument is not <see langword="null"/>.
        /// </summary>
        /// <param name="previous">The page to which the "Back" button should lead.</param>
        /// <param name="update">The incoming update.</param>
        /// <returns>The built ready-to-print message.</returns>
        public Task<ITelegramMessage> BuildMessageAsync(IBotPage? previous, ISignedUpdate update);
    }
}