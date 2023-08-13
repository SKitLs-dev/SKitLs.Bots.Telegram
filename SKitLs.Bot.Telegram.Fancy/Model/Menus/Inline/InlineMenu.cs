using SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Inline
{
    /// <summary>
    /// Represents a specific type of inline menu. This menu is designed to be displayed as an inline message's menu.
    /// </summary>
    public class InlineMenu : InlineBase
    {
        /// <summary>
        /// A private list used to store the inline buttons to be added to the menu.
        /// </summary>
        private List<IInlineButton> Buttons { get; set; } = new();

        /// <summary>
        /// Serializer used to simplify the addition process.
        /// </summary>
        public IArgsSerializeService? Serializer { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="InlineMenu"/> with default data.
        /// </summary>
        public InlineMenu() { }

        /// <summary>
        /// Creates a new instance of <see cref="InlineMenu"/>, resolving <see cref="IArgsSerializeService"/>
        /// from specified <see cref="BotManager"/> instance.
        /// </summary>
        public InlineMenu(BotManager executer) : this(executer.ResolveService<IArgsSerializeService>()) { }

        /// <summary>
        /// Creates a new instance of <see cref="InlineMenu"/> with a specified <see cref="IArgsSerializeService"/>.
        /// </summary>
        public InlineMenu(IArgsSerializeService serializer) => Serializer = serializer;

        private InlineMenu(List<IInlineButton> buttons, IArgsSerializeService? serializer)
        {
            Buttons = buttons ?? throw new ArgumentNullException(nameof(buttons));
            Serializer = serializer;
        }


        /// <summary>
        /// Adds a new default callback to the menu.
        /// </summary>
        /// <param name="callback">The argument callback to add.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        public void Add(DefaultCallback callback, bool singleLine = false) => Add(callback.Label, callback.GetSerializedData(), singleLine);

        /// <summary>
        /// Adds a new argument callback to the menu.
        /// </summary>
        /// <typeparam name="T">Represents the type of action's argument.</typeparam>
        /// <param name="callback">The argument callback to add.</param>
        /// <param name="data">The argument to be implemented.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        /// <exception cref="NullReferenceException"></exception>
        public void Add<T>(BotArgedCallback<T> callback, T data, bool singleLine = false) where T : notnull, new()
            => Add(callback.Label, callback.GetSerializedData(data, Serializer ?? throw new NullReferenceException(nameof(Serializer))), singleLine);

        /// <summary>
        /// Adds a custom bot action to the menu.
        /// </summary>
        /// <param name="actionLabel">The custom label for the action.</param>
        /// <param name="callback">The action to add.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        public void Add(string actionLabel, IBotAction<SignedCallbackUpdate> callback, bool singleLine = false)
            => Add(actionLabel, callback.GetSerializedData(), singleLine);

        /// <summary>
        /// Adds a custom argument action to the menu.
        /// </summary>
        /// <typeparam name="T">Represents the type of action's argument.</typeparam>
        /// <param name="customLabel">The custom label for the action.</param>
        /// <param name="callback">The action to add.</param>
        /// <param name="data">The argument to be implemented.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        /// <exception cref="NullReferenceException"></exception>
        public void Add<T>(string customLabel, IArgedAction<T, SignedCallbackUpdate> callback, T data, bool singleLine = false) where T : notnull
            => Add(customLabel, callback.GetSerializedData(data, Serializer ?? throw new NullReferenceException(nameof(Serializer))), singleLine);

        /// <summary>
        /// Adds a new labeled data pair to the menu.
        /// </summary>
        /// <param name="data">The labeled data pair to add.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        public void Add(LabeledData data, bool singleLine = false) => Add(data.Label, data.Data, singleLine);

        /// <summary>
        /// Adds a new data pair to the menu.
        /// </summary>
        /// <param name="label">The label for the callback.</param>
        /// <param name="data">The data for the callback.</param>
        /// <param name="singleLine">Determines whether the button should be placed on a single line.</param>
        public void Add(string label, string data, bool singleLine = false) => Add(new InlineButton(label, data, singleLine));

        /// <summary>
        /// Adds a new inline button to the menu.
        /// </summary>
        /// <param name="button">The inline button to add.</param>
        public void Add(IInlineButton button) => Buttons.Add(button);

        /// <inheritdoc/>
        protected override List<IInlineButton> GetButtons() => Buttons;

        /// <inheritdoc/>
        public override object Clone()
        {
            var buttons = new List<IInlineButton>();
            foreach (var button in GetButtons())
                buttons.Add(button is ICloneable clone ? (IInlineButton)clone.Clone() : button);
            return new InlineMenu(buttons, Serializer) { ColumnsCount = ColumnsCount };
        }

        /// <summary>
        /// Combines the interiors of two <see cref="InlineMenu"/> instances.
        /// </summary>
        /// <param name="left">The first menu to combine.</param>
        /// <param name="right">The second menu to combine.</param>
        /// <returns>A new <see cref="InlineMenu"/> instance containing data from both <paramref name="left"/> and <paramref name="right"/> menus.</returns>
        public static InlineMenu operator +(InlineMenu left, InlineMenu right)
        {
            var buttons = new List<IInlineButton>();
            buttons.AddRange(left.Buttons);
            buttons.AddRange(right.Buttons);
            return new InlineMenu(buttons, left.Serializer ?? right.Serializer);
        }
    }
}