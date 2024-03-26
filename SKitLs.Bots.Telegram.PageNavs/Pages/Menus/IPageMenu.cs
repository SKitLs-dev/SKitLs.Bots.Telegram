using SKitLs.Bots.Telegram.AdvancedMessages.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Pages;

namespace SKitLs.Bots.Telegram.PageNavs.Pages.Menus
{
    /// <summary>
    /// An interface that provides methods for representing a page's menu, which <see cref="IBotPage"/> can work with.
    /// <para/>
    /// For the default implementation, see: <see cref="PageNavMenu"/>.
    /// </summary>
    public interface IPageMenu
    {
        private static string _navLabelMask = "{0} >";

        /// <summary>
        /// Special mask used to highlight navigation buttons.
        /// The value to be set should contain "{0}" substring for formatting.
        /// If it does not, the encapsulated field will not be updated.
        /// </summary>
        public static string NavigationLabelMask
        {
            get => _navLabelMask;
            set => _navLabelMask = value.Contains("{0}") ? value : _navLabelMask;
        }

        /// <summary>
        /// Asynchronously converts an instance of a custom <see cref="IPageMenu"/> to the specified <see cref="IMessageMenu"/>
        /// that can be integrated into an instance of <see cref="IOutputMessage"/>.
        /// </summary>
        /// <param name="previous">The page to which the "Back" button should lead.</param>
        /// <param name="owner">The current page that owns the menu.</param>
        /// <param name="update">The incoming update.</param>
        /// <returns>The built ready-to-use menu.</returns>
        public Task<IBuildableContent<IMessageMenu>> BuildAsync(IBotPage? previous, IBotPage owner, ISignedUpdate update);
    }
}