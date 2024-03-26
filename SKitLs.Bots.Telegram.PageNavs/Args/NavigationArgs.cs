using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Pages;

namespace SKitLs.Bots.Telegram.PageNavs.Args
{
    /// <summary>
    /// Represents special arguments wrapper for an <see cref="IArgedAction{TArg, TUpdate}"/> used in <see cref="IMenuService"/>.
    /// It carries data about menu navigation parameters, including the requesting page and determines whether a session should be refreshed.
    /// </summary>
    public class NavigationArgs
    {
        /// <summary>
        /// Gets or sets the page requested by an open callback.
        /// </summary>
        [BotActionArgument(0)]
        public IBotPage Page { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the user's session data should be refreshed.
        /// </summary>
        [BotActionArgument(1)]
        public bool Refresh { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationArgs"/> class with default values.
        /// This constructor is used in <see cref="IArgsSerializeService"/> by default.
        /// <para/>
        /// <b>Not recommended to use in your code.</b>
        /// </summary>
        public NavigationArgs() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationArgs"/> class with the specified data.
        /// </summary>
        /// <param name="page">The requesting page.</param>
        /// <param name="refresh">Determines whether the user's session data should be refreshed.</param>
        public NavigationArgs(IBotPage page, bool refresh = false)
        {
            Page = page;
            Refresh = refresh;
        }
    }
}