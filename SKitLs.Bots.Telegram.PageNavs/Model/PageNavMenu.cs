using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
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

        /// <inheritdoc/>
        public void PathTo(params IBotPage[] pages) => pages.ToList().ForEach(p => PagesLinks.Add(p));

        /// <inheritdoc/>
        public bool TryRemove(IBotPage page)
        {
            var existing = PagesLinks.Find(x => page.PageId == x.PageId);
            return existing is not null && PagesLinks.Remove(existing);
        }

        /// <summary>
        /// <b>Optional.</b> A special menu that an "Exit" Button leads to. Exiting process should
        /// refresh user's <see cref="PageSessionData"/>, setting root page to <c><see cref="ExitButtonLink"/></c> instance.
        /// Can be set via <see cref="ExitTo(IBotPage?)"/>.
        /// </summary>
        public IBotPage? ExitButtonLink { get; private set; }

        /// <inheritdoc/>
        public void ExitTo(IBotPage? page) => ExitButtonLink = page;

        /// <summary>
        /// A list of callbacks that should be append to the navigation buttons array.
        /// </summary>
        public List<LabeledData> Actions { get; } = new();

        /// <inheritdoc/>
        public void AddAction(LabeledData actionData) => Actions.Add(actionData);

        /// <summary>
        /// Adds new non-navigation action with a certain <paramref name="action"/> to the inline menu.
        /// Automatically gets callback's action base via its <see cref="IBotAction.GetSerializedData(string[])"/>.
        /// </summary>
        /// <param name="action">Callback to add.</param>
        public void AddAction(DefaultCallback action) => Actions.Add(new(action.Label, action.GetSerializedData()));

        /// <inheritdoc/>
        /// <remarks>
        /// Convert result is: <see cref="InlineMenu"/>.
        /// <para/>
        /// To update default "Back" ad "Exit" buttons' labels use <c>"lang.pageNavs.json"</c> settings files.
        /// To update localization keys use <see cref="PNSettings"/> class.
        /// </remarks>
        public async Task<IBuildableContent<IMessageMenu>> BuildAsync(IBotPage? previous, IBotPage owner, ISignedUpdate update)
        {
            var mm = update.Owner.ResolveService<IMenuManager>();

            var res = new InlineMenu(update.Owner)
            {
                ColumnsCount = ColumnsCount,
            };
            PagesLinks.ForEach(page => res.Add(string.Format(IPageMenu.NavigationLabelMask, page.GetLabel(update)), mm.OpenPageCallback, new(page)));
            Actions.ForEach(act => res.Add(act));

            if (previous is not null)
                res.Add(update.Owner.ResolveBotString(PNSettings.BackButtonLocalKey), mm.BackCallback, true);
            if (ExitButtonLink is not null)
                res.Add(update.Owner.ResolveBotString(PNSettings.ExitButtonLocalKey), mm.OpenPageCallback, new(ExitButtonLink, true), true);

            return await Task.FromResult(res);
        }
    }
}