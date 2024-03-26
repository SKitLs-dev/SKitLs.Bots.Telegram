using SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Settings;

namespace SKitLs.Bots.Telegram.PageNavs.Pages.Menus
{
    /// <summary>
    /// Default implementation of the <see cref="IPageNavMenu"/> interface that provides methods for creating a page's menu
    /// with integrated navigation functionality.
    /// </summary>
    public class PageNavMenu : IPageNavMenu
    {
        private int _columnsCount = 1;

        /// <summary>
        /// The number of columns that should be printed in one row.
        /// </summary>
        public int ColumnsCount
        {
            get => _columnsCount;
            set => _columnsCount = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(ColumnsCount));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the menu should automatically localize its buttons.
        /// </summary>
        public bool AutomaticallyLocalize { get; set; } = true;

        /// <summary>
        /// Determines whether a "Back" button would be printed in the menu.
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
        /// <b>Optional.</b> A special menu that an "Exit" button leads to. The exiting process should
        /// refresh the user's <see cref="PageSessionData"/>, setting the root page to the <c><see cref="ExitButtonLead"/></c> instance.
        /// Can be set via <see cref="ExitTo(IBotPage?)"/>.
        /// </summary>
        public IBotPage? ExitButtonLead { get; private set; }

        /// <inheritdoc/>
        public void ExitTo(IBotPage? page) => ExitButtonLead = page;

        /// <summary>
        /// A list of callbacks that should be appended to the navigation buttons array.
        /// </summary>
        public List<IBuildableContent<IInlineButton>> Actions { get; } = [];

        /// <summary>
        /// Adds a new non-navigation action with the specified <paramref name="actionData"/> to the inline menu.
        /// </summary>
        /// <param name="actionData">The labeled data of a callback to add.</param>
        public void AddAction(IInlineButton actionData) => AddAction(new SelfBuild<IInlineButton>(actionData));

        /// <summary>
        /// Adds a new non-navigation action with the specified <paramref name="actionData"/> to the inline menu.
        /// </summary>
        /// <param name="actionData">The labeled data of a callback to add.</param>
        public void AddAction(IBuildableContent<IInlineButton> actionData) => Actions.Add(actionData);

        /// <inheritdoc/>
        public void AddAction(LabeledData actionData) => AddAction((IInlineButton)new InlineButton(actionData.Label, actionData.Data));

        /// <summary>
        /// Adds a new non-navigation action with a certain <paramref name="action"/> to the inline menu.
        /// Automatically gets the callback's action base via its <see cref="IBotAction.GetSerializedData(string[])"/>.
        /// </summary>
        /// <param name="action">The callback to add.</param>
        public void AddAction(DefaultCallback action)
        {
            var button = new InlineButton(action.Label, action.GetSerializedData());
            if (AutomaticallyLocalize)
                AddAction(Localize.Inline(button));
            else
                AddAction((IInlineButton)button);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The conversion result is an <see cref="InlineMenu"/>.
        /// <para/>
        /// To update the default labels of the "Back" and "Exit" buttons, use the "lang.pageNavs.json" settings files.
        /// To update the localization keys, use the <see cref="PNSettings"/> class.
        /// </remarks>
        public async Task<IBuildableContent<IMessageMenu>> BuildAsync(IBotPage? previous, IBotPage owner, ISignedUpdate update)
        {
            var menuService = update.Owner.ResolveService<IMenuService>();

            var menu = new InlineMenu(update.Owner, AutomaticallyLocalize, ColumnsCount);
            foreach (var page in PagesLinks)
            {
                var redirectLabel = string.Format(IPageMenu.NavigationLabelMask, await page.BuildLabelAsync(update));
                menu.Add(redirectLabel, menuService.OpenPageCallback, new(page));
            }
            foreach (var button in Actions)
            {
                menu.Add(button);
            }

            if (previous is not null)
                menu.Add(PNSettings.BackButtonLocalKey, menuService.BackCallback, true);
            if (ExitButtonLead is not null)
                menu.Add(PNSettings.ExitButtonLocalKey, menuService.OpenPageCallback, new(ExitButtonLead, true), true);

            return menu;
        }
    }
}