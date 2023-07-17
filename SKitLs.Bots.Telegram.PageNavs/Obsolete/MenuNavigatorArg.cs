using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Obsolete
{
    [Obsolete("The previous iteration of data args. Will be removed in future versions. Use NavigationArgs instead.", true)]
    public class MenuNavigatorArg
    {
        /// <summary>
        /// <see cref="IMesPage"/> that should be opened. Used in "Back" Button
        /// </summary>
        [BotActionArgument(0)]
        public string RequestedPage { get; set; } = null!;

        /// <summary>
        /// <see cref="IMesPage"/> that caused this page menu. Used in "Back" Button
        /// </summary>
        [BotActionArgument(1)]
        public string PreviousPage { get; set; } = null!;
    }
}