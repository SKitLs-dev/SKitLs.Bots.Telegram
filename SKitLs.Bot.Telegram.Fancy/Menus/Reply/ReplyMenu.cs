using SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Reply;
using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using System.Collections;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Menus.Reply
{
    /// <summary>
    /// Represents a specific type of <see cref="ReplyKeyboardMarkup"/> menu.
    /// This menu is displayed instead of the keyboard to simplify data input.
    /// </summary>
    public class ReplyMenu : ReplyBase, IEnumerable<IBuildableContent<IReplyButton>>
    {
        /// <summary>
        /// A private list used to store the buttons to be added to the menu.
        /// </summary>
        private List<IBuildableContent<IReplyButton>> Buttons { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether the menu should automatically localize its buttons.
        /// </summary>
        public bool AutomaticallyLocalize { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyMenu"/> class with default data.
        /// </summary>
        /// <param name="localize">Optional. Specifies whether the menu should automatically localize its buttons.</param>
        /// <param name="columnsCount">The number of columns in the menu. Default is <c>1</c>.</param>
        public ReplyMenu(bool localize = false, int columnsCount = 1) : base(columnsCount)
        {
            AutomaticallyLocalize = localize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyMenu"/> class with specified buttons.
        /// </summary>
        /// <param name="buttons">The list of buttons to add to the menu.</param>
        /// <param name="columnsCount">The number of columns in the menu. Default is <c>1</c>.</param>
        public ReplyMenu(List<IBuildableContent<IReplyButton>> buttons, int columnsCount = 1) : base(columnsCount) => Buttons = buttons;

        /// <summary>
        /// Adds a new button to the menu.
        /// </summary>
        /// <param name="reply">The text for the button to add.</param>
        /// <param name="format">Optional. The format arguments used for formatting the localized text.</param>
        public void Add(string reply, params string?[] format)
        {
            var button = new ReplyButton(reply);
            if (AutomaticallyLocalize)
                Buttons.Add(Localize.Reply(button, format));
            else
                Buttons.Add(button);
        }

        /// <summary>
        /// Adds a new button to the menu.
        /// </summary>
        /// <param name="button">The button to add.</param>
        public void Add(IReplyButton button) => Add(new SelfBuild<IReplyButton>(button));

        /// <summary>
        /// Adds a new button to the menu.
        /// </summary>
        /// <param name="buildableButton">The buildable button to add.</param>
        public void Add(IBuildableContent<IReplyButton> buildableButton) => Buttons.Add(buildableButton);

        /// <summary>
        /// Adds several buttons to the menu. Uses <see cref="ReplyButton"/> by default.
        /// </summary>
        /// <param name="replies">The list of button texts to add.</param>
        public void AddRange(List<string> replies) => replies.ForEach(r => Add(r));

        /// <summary>
        /// Adds several buttons to the menu.
        /// </summary>
        /// <param name="buttons">The list of buttons to add.</param>
        public void AddRange(List<IReplyButton> buttons) => buttons.ForEach(Add);

        /// <inheritdoc/>
        protected override List<IBuildableContent<IReplyButton>> GetButtons() => Buttons;

        /// <inheritdoc/>
        public override object Clone()
        {
            var buttons = new List<IBuildableContent<IReplyButton>>();
            foreach (var button in GetButtons())
                buttons.Add((IBuildableContent<IReplyButton>)button.Clone());
            return new ReplyMenu(buttons) { ColumnsCount = ColumnsCount };
        }

        /// <inheritdoc/>
        public IEnumerator<IBuildableContent<IReplyButton>> GetEnumerator() => Buttons.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Combines the interiors of two <see cref="ReplyMenu"/> instances.
        /// </summary>
        /// <param name="left">The first menu to combine.</param>
        /// <param name="right">The second menu to combine.</param>
        /// <returns>A new <see cref="ReplyMenu"/> instance containing data from both <paramref name="left"/> and <paramref name="right"/> menus.</returns>
        public static ReplyMenu operator +(ReplyMenu left, ReplyMenu right)
        {
            var buttons = new List<IBuildableContent<IReplyButton>>();
            buttons.AddRange(left.Buttons);
            buttons.AddRange(right.Buttons);
            return new ReplyMenu(buttons);
        }
    }
}