using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using SKitLs.Bots.Telegram.PageNavs.resources.settings;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// Default realization of an <see cref="IPageNavMenu"/> interface that provides methods of creating page's menu,
    /// with integrated navigation functionality.
    /// </summary>
    public class PageNavMenu : IPageNavMenu
    {
        /// <summary>
        /// The amount of columns that should be printed in one row.
        /// </summary>
        public int ColumnsCount { get; set; } = 1;

        /// <summary>
        /// Determines whether a "Back" Button would be printed in the menu.
        /// </summary>
        public bool EnableBackButton { get; set; } = true;

        /// <summary>
        /// A list of pages that this page leads to.
        /// </summary>
        public List<IBotPage> PagesLinks { get; } = new();

        /// <summary>
        /// Adds links to specified <paramref name="pages"/> to the menu.
        /// </summary>
        /// <param name="pages">The array of pages that menu should link to.</param>
        public void PathTo(params IBotPage[] pages) => pages.ToList().ForEach(p => PagesLinks.Add(p));

        /// <summary>
        /// Tries to remove a certain page from menu's navigation links and returns the result of an attempt.
        /// </summary>
        /// <param name="page">Page to remove. Uses <see cref="IBotPage.PageId"/> for comparison.</param>
        /// <returns><see langword="true"/> if an item was found and removed. Otherwise <see langword="false"/>.</returns>
        public bool TryRemove(IBotPage page)
        {
            var existing = PagesLinks.Find(x => page.PageId == x.PageId);
            return existing is not null && PagesLinks.Remove(existing);
        }

        /// <summary>
        /// Optional. A special menu that an "Exit" Button leads to. Exiting process should
        /// refresh user's <see cref="PageSessionData"/>, setting root page to <c><see cref="ExitButtonLink"/></c> instance.
        /// Can be set via <see cref="ExitTo(IBotPage?)"/>.
        /// </summary>
        public IBotPage? ExitButtonLink { get; private set; }

        /// <summary>
        /// Adds special "Exit" Button, that refreshes user's session setting <paramref name="page"/> as a new root page.
        /// </summary>
        /// <param name="page">Page that "Exit" button should lead to.</param>
        public void ExitTo(IBotPage? page) => ExitButtonLink = page;

        /// <summary>
        /// A list of callbacks that should be append to the navigation buttons array.
        /// </summary>
        public List<LabeledData> Actions { get; } = new();

        /// <summary>
        /// Adds new non-navigation action with a certain <paramref name="actionData"/> to the inline menu.
        /// </summary>
        /// <param name="actionData">Labeled data of a callback to add.</param>
        public void AddAction(LabeledData actionData) => Actions.Add(actionData);

        /// <summary>
        /// Adds new non-navigation action with a certain <paramref name="action"/> to the inline menu.
        /// Automatically gets callback's action base via its <see cref="IBotAction.GetSerializedData(string[])"/>.
        /// </summary>
        /// <param name="action">Callback to add.</param>
        public void AddAction(DefaultCallback action) => Actions.Add(new(action.Label, action.GetSerializedData()));

        /// <summary>
        /// Converts an in instance of a <c><see cref="PageNavMenu"/> : <see cref="IPageMenu"/></c> to the specified
        /// <see cref="PairedInlineMenu"/> that can be integrated to an instance of <see cref="IOutputMessage"/>.
        /// <para>
        /// To update default "Back" ad "Exit" buttons' labels use <c>"lang.pageNavs.json"</c> settings files.
        /// To update localization keys use <see cref="PNSettings"/> class.
        /// </para>
        /// </summary>
        /// <param name="previous">A page to which should lead "Back" Button.</param>
        /// <param name="owner">Current page that owns menu.</param>
        /// <param name="update">An incoming update.</param>
        /// <returns>Built ready-to-use menu.</returns>
        public IMesMenu Build(IBotPage? previous, IBotPage owner, ISignedUpdate update)
        {
            var mm = update.Owner.ResolveService<IMenuManager>();

            var res = new PairedInlineMenu()
            {
                Serializer = update.Owner.ResolveService<IArgsSerializeService>(),
                ColumnsCount = ColumnsCount,
            };
            PagesLinks.ForEach(page => res.Add(string.Format(IPageMenu.NavigationLabelMask, page.GetLabel(update)), mm.OpenPageCallback, new(page)));
            Actions.ForEach(act => res.Add(act));

            if (previous is not null)
                res.Add(update.Owner.ResolveBotString(PNSettings.BackButtonLocalKey), mm.BackCallback, true);
            if (ExitButtonLink is not null)
                res.Add(update.Owner.ResolveBotString(PNSettings.ExitButtonLocalKey), mm.OpenPageCallback, new(ExitButtonLink, true), true);

            return res;
        }
    }
}