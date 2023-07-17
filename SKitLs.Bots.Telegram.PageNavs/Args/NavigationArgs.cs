using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Args
{
    /// <summary>
    /// Special arguments wrapper for an <see cref="IArgedAction{TArg, TUpdate}"/>, used in <see cref="IMenuManager"/>
    /// that carries data about menu's navigation parameters.
    /// Contains requesting page and determines whether a session should be refreshed.
    /// </summary>
    public class NavigationArgs
    {
        /// <summary>
        /// A page requested by an open callback.
        /// </summary>
        [BotActionArgument(0)]
        public IBotPage Page { get; set; } = null!;
        /// <summary>
        /// Determines whether user's session data should be refreshed.
        /// </summary>
        [BotActionArgument(1)]
        public bool Refresh { get; set; } = false;

        /// <summary>
        /// Creates a new instance of <see cref="NavigationArgs"/> with <see langword="null"/> fields.
        /// Used in <see cref="IArgsSerilalizerService"/> by default.
        /// <para>
        /// <c>Not recommended to use in your code.</c>
        /// </para>
        /// </summary>
        public NavigationArgs() { }
        /// <summary>
        /// Creates a new instance of <see cref="NavigationArgs"/> with specified data.
        /// </summary>
        /// <param name="page">Requesting page.</param>
        /// <param name="refresh">Whether user's session data should be refreshed.</param>
        public NavigationArgs(IBotPage page, bool refresh = false)
        {
            Page = page;
            Refresh = refresh;
        }
    }
}