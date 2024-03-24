using SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using System.Collections;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Menus.Inline
{
    /// <summary>
    /// Represents a specific type of inline menu. This menu is designed to be displayed as an inline message's menu.
    /// </summary>
    public class InlineMenu : InlineBase, IEnumerable<IBuildableContent<IInlineButton>>
    {
        /// <summary>
        /// A private list used to store the inline buttons to be added to the menu.
        /// </summary>
        private List<IBuildableContent<IInlineButton>> Buttons { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether the menu should automatically localize its buttons.
        /// </summary>
        public bool AutomaticallyLocalize { get; set; } = false;

        /// <summary>
        /// Serializer used to simplify the addition process.
        /// </summary>
        public IArgsSerializeService? Serializer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineMenu"/> class with default data.
        /// </summary>
        /// <param name="localize">Optional. Specifies whether the menu should automatically localize its buttons.</param>
        /// <param name="columnsCount">The number of columns in the menu.</param>
        public InlineMenu(bool localize = false, int columnsCount = 1) : base(columnsCount)
        {
            AutomaticallyLocalize = localize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineMenu"/> class,
        /// resolving the <see cref="IArgsSerializeService"/> from the specified <see cref="BotManager"/> instance.
        /// </summary>
        /// <param name="executer">The <see cref="BotManager"/> instance, used for resolving services.</param>
        /// <param name="columnsCount">The number of columns in the menu.</param>
        public InlineMenu(BotManager executer, int columnsCount = 1) : this(executer.ResolveService<IArgsSerializeService>(), columnsCount) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineMenu"/> class with a specified <see cref="IArgsSerializeService"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="IArgsSerializeService"/> instance used for serializing menu buttons.</param>
        /// <param name="columnsCount">The number of columns in the menu.</param>
        public InlineMenu(IArgsSerializeService serializer, int columnsCount = 1) : base(columnsCount) => Serializer = serializer;

        private InlineMenu(List<IBuildableContent<IInlineButton>> buttons, IArgsSerializeService? serializer, int columnsCount) : base(columnsCount)
        {
            Buttons = buttons ?? throw new ArgumentNullException(nameof(buttons));
            Serializer = serializer;
        }

        /// <summary>
        /// Adds a new default callback to the menu.
        /// </summary>
        /// <param name="callback">The argument callback to add.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        /// <param name="format">Optional formatting arguments for localization.</param>
        public void Add(DefaultCallback callback, bool singleLine = false, params string?[] format) => Add(callback.Label, callback.GetSerializedData(), singleLine, format);

        /// <summary>
        /// Adds a new argument callback to the menu.
        /// </summary>
        /// <typeparam name="T">Represents the type of action's argument.</typeparam>
        /// <param name="callback">The argument callback to add.</param>
        /// <param name="data">The argument to be implemented.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        /// <param name="format">Optional formatting arguments for localization.</param>
        /// <exception cref="NullReferenceException"></exception>
        public void Add<T>(BotArgedCallback<T> callback, T data, bool singleLine = false, params string?[] format) where T : notnull, new()
            => Add(callback.Label, callback.GetSerializedData(data, Serializer ?? throw new NullReferenceException(nameof(Serializer))), singleLine, format);

        /// <summary>
        /// Adds a custom bot action to the menu.
        /// </summary>
        /// <param name="actionLabel">The custom label for the action.</param>
        /// <param name="callback">The action to add.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        /// <param name="format">Optional formatting arguments for localization.</param>
        public void Add(string actionLabel, IBotAction<SignedCallbackUpdate> callback, bool singleLine = false, params string?[] format)
            => Add(actionLabel, callback.GetSerializedData(), singleLine, format);

        /// <summary>
        /// Adds a custom argument action to the menu.
        /// </summary>
        /// <typeparam name="T">Represents the type of action's argument.</typeparam>
        /// <param name="customLabel">The custom label for the action.</param>
        /// <param name="callback">The action to add.</param>
        /// <param name="data">The argument to be implemented.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        /// <param name="format">Optional formatting arguments for localization.</param>
        /// <exception cref="NullReferenceException"></exception>
        public void Add<T>(string customLabel, IArgedAction<T, SignedCallbackUpdate> callback, T data, bool singleLine = false, params string?[] format) where T : notnull
            => Add(customLabel, callback.GetSerializedData(data, Serializer ?? throw new NullReferenceException(nameof(Serializer))), singleLine, format);

        /// <summary>
        /// Adds a new labeled data pair to the menu.
        /// </summary>
        /// <param name="data">The labeled data pair to add.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        /// <param name="format">Optional formatting arguments for localization.</param>
        public void Add(LabeledData data, bool singleLine = false, params string?[] format) => Add(data.Label, data.Data, singleLine, format);

        /// <summary>
        /// Adds a new data pair to the menu.
        /// </summary>
        /// <param name="label">The label for the callback.</param>
        /// <param name="data">The data for the callback.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        /// <param name="format">Optional formatting arguments for localization.</param>
        public void Add(string label, string data, bool singleLine = false, params string?[] format)
        {
            var button = new InlineButton(label, data, singleLine);
            if (AutomaticallyLocalize)
                Add(Localize.Inline(button, format));
            else
                Add((IInlineButton)button);
        }

        /// <summary>
        /// Adds a new inline button to the menu.
        /// </summary>
        /// <param name="button">The inline button to add.</param>
        public void Add(IInlineButton button) => Add(new SelfBuild<IInlineButton>(button));

        /// <summary>
        /// Adds a new inline button to the menu.
        /// </summary>
        /// <param name="button">The buildable inline button to add.</param>
        public void Add(IBuildableContent<IInlineButton> button) => Buttons.Add(button);

        /// <inheritdoc/>
        protected override List<IBuildableContent<IInlineButton>> GetButtons() => Buttons;

        /// <inheritdoc/>
        public override object Clone()
        {
            var buttons = new List<IBuildableContent<IInlineButton>>();
            foreach (var button in GetButtons())
                buttons.Add((IBuildableContent<IInlineButton>)button.Clone());
            return new InlineMenu(buttons, Serializer, ColumnsCount);
        }

        /// <inheritdoc/>
        public IEnumerator<IBuildableContent<IInlineButton>> GetEnumerator() => Buttons.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Sets the number of columns for the inline menu.
        /// </summary>
        /// <param name="columnCount">The number of columns to set.</param>
        /// <returns>The current <see cref="InlineMenu"/> instance.</returns>
        public InlineMenu SetColumns(int columnCount)
        {
            ColumnsCount = columnCount;
            return this;
        }

        /// <summary>
        /// Combines the interiors of two <see cref="InlineMenu"/> instances.
        /// </summary>
        /// <param name="left">The first menu to combine.</param>
        /// <param name="right">The second menu to combine.</param>
        /// <returns>A new <see cref="InlineMenu"/> instance containing data from both <paramref name="left"/> and <paramref name="right"/> menus.</returns>
        public static InlineMenu operator +(InlineMenu left, InlineMenu right)
        {
            var buttons = new List<IBuildableContent<IInlineButton>>();
            buttons.AddRange(left.Buttons);
            buttons.AddRange(right.Buttons);
            return new InlineMenu(buttons, left.Serializer ?? right.Serializer, Math.Max(left.ColumnsCount, right.ColumnsCount));
        }
    }
}