using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model;

namespace SKitLs.Bots.Telegram.PageNavs.Prototype
{
    /// <summary>
    /// An interface that provides methods for the representation of a page's menu, that <see cref="IBotPage"/> can work with.
    /// <para>
    /// For default realization see: <see cref="PageNavMenu"/>.
    /// </para>
    /// </summary>
    public interface IPageMenu
    {
        private static string _navLabelMask = "{0} >";
        /// <summary>
        /// Special mask that used to highlight navigation buttons.
        /// Value to be set should contain "{0}" substring for formatting.
        /// If does not, incapsulated field will not be updated.
        /// </summary>
        public static string NavigationLabelMask
        {
            get => _navLabelMask;
            set => _navLabelMask = value.Contains("{0}") ? value : _navLabelMask;
        }

        /// <summary>
        /// Asynchronously converts an in instance of a custom <see cref="IPageMenu"/> to the specified <see cref="IMessageMenu"/>
        /// that can be integrated to an instance of <see cref="IOutputMessage"/>.
        /// </summary>
        /// <param name="previous">A page to which should lead "Back" Button.</param>
        /// <param name="owner">Current page that owns menu.</param>
        /// <param name="update">An incoming update.</param>
        /// <returns>Built ready-to-use menu.</returns>
        public Task<IBuildableContent<IMessageMenu>> BuildAsync(IBotPage? previous, IBotPage owner, ISignedUpdate update);
    }
}