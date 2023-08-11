using SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Reply;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Reply
{
    /// <summary>
    /// Represents a specific type of <see cref="ReplyKeyboardMarkup"/> menu.
    /// This menu is displayed instead of the keyboard to simplify data input.
    /// <para/>
    /// Derived from <see cref="ReplyBase"/>.
    /// <inheritdoc/>
    /// </summary>
    public class ReplyMenu : ReplyBase
    {
        /// <summary>
        /// A private list used to store the buttons to be added to the menu.
        /// </summary>
        private List<IReplyButton> Buttons { get; set; } = new();

        /// <summary>
        /// Creates a new instance of the <see cref="ReplyMenu"/> class with default data.
        /// </summary>
        public ReplyMenu() { }
        /// <summary>
        /// Creates a new instance of the <see cref="ReplyMenu"/> class with specified buttons.
        /// </summary>
        /// <param name="buttons">The list of buttons to add to the menu.</param>
        public ReplyMenu(List<IReplyButton> buttons) => Buttons = buttons;

        /// <summary>
        /// Adds a new button to the menu.
        /// </summary>
        /// <param name="reply">The text for the button to add.</param>
        public void AddRange(string reply) => Buttons.Add(new ReplyButton(reply));
        /// <summary>
        /// Adds a new button to the menu.
        /// </summary>
        /// <param name="button">The button to add.</param>
        public void Add(IReplyButton button) => Buttons.Add(button);
        /// <summary>
        /// Adds several buttons to the menu. Uses <see cref="ReplyButton"/> by default.
        /// </summary>
        /// <param name="replies">The list of button texts to add.</param>
        public void AddRange(List<string> replies) => Buttons.AddRange(replies.Select(x => new ReplyButton(x)).ToList());
        /// <summary>
        /// Adds several buttons to the menu.
        /// </summary>
        /// <param name="buttons">The list of buttons to add.</param>
        public void Add(List<IReplyButton> buttons) => Buttons.AddRange(buttons);

        /// <inheritdoc/>
        protected override List<IReplyButton> GetButtons() => Buttons;

        /// <summary>
        /// Combines the interiors of two <see cref="ReplyMenu"/> instances.
        /// </summary>
        /// <param name="left">The first menu to combine.</param>
        /// <param name="right">The second menu to combine.</param>
        /// <returns>A new <see cref="ReplyMenu"/> instance containing data from both <paramref name="left"/> and <paramref name="right"/> menus.</returns>
        public static ReplyMenu operator +(ReplyMenu left, ReplyMenu right)
        {
            var buttons = new List<IReplyButton>();
            buttons.AddRange(left.Buttons);
            buttons.AddRange(right.Buttons);
            return new ReplyMenu(buttons);
        }
    }
}