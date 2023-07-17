using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model;

namespace SKitLs.Bots.Telegram.PageNavs.Prototype
{
    /// <summary>
    /// An interface that provides methods for the representation of a menu page instance,
    /// that <see cref="IMenuManager"/> can work with.
    /// <para>
    /// For default realizations see: <see cref="StaticPage"/>, <see cref="WidgetPage"/>.
    /// </para>
    /// </summary>
    public interface IBotPage : IArgPackable
    {
        /// <summary>
        /// An unique page's identifier.
        /// </summary>
        public string PageId { get; }
        /// <summary>
        /// Page's menu.
        /// </summary>
        public IPageMenu? Menu { get; }

        /// <summary>
        /// Returns a string that should be printed on the inline keyboard menu as a navigation label.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>A string that represents an instance as a navigation label.</returns>
        public string GetLabel(ISignedUpdate update);
        /// <summary>
        /// Converts page data to a printable <see cref="IOutputMessage"/> that should be printed
        /// based on incoming <paramref name="update"/>.
        /// Optionally can add "Back" Button if <paramref name="previous"/> argument is not <see langword="null"/>.
        /// </summary>
        /// <param name="previous">A page to which should lead "Back" Button.</param>
        /// <param name="update">An incoming update.</param>
        /// <returns>Built ready-to-print message.</returns>
        public IOutputMessage BuildMessage(IBotPage? previous, ISignedUpdate update);
    }
}