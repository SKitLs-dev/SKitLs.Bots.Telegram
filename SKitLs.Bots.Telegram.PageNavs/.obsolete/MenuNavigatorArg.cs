using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Args;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Obsolete
{
    /// <summary>
    /// <b>Obsolete.</b> Replaced with <see cref="NavigationArgs"/>.
    /// </summary>
    [Obsolete("The previous iteration of data args. Will be removed in future versions. Use NavigationArgs instead.", true)]
    public class MenuNavigatorArg
    {
        /// <summary>
        /// <see cref="IPageMenu"/> that should be opened.
        /// </summary>
        [BotActionArgument(0)]
        public string RequestedPage { get; set; } = null!;

        /// <summary>
        /// <see cref="IPageMenu"/> that caused this page menu. Used in "Back" Button
        /// </summary>
        [BotActionArgument(1)]
        public string PreviousPage { get; set; } = null!;
    }
}