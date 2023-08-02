using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    /// <summary>
    /// Specific menu that realizes the reply one. Displayed instead of keyboard to simplify data input.
    /// </summary>
    public class ReplyMenu : IMesMenu
    {
        /// <summary>
        /// Defines menu's columns count.
        /// </summary>
        public int ColumnsCount { get; set; } = 1;
        /// <summary>
        /// Specific temporary container that stores buttons to add.
        /// </summary>
        private List<string> Buttons { get; set; } = new();
        /// <summary>
        /// Determines either app's keyboard should be resized.
        /// More info: <see cref="ReplyKeyboardMarkup.ResizeKeyboard"/>.
        /// </summary>
        public bool ResizeKeyboard { get; set; } = true;
        /// <summary>
        /// Determines either app's keyboard should disappear after selection.
        /// More info: <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>.
        /// </summary>
        public bool OneTimeKeyboard { get; set; } = true;

        /// <summary>
        /// Creates a new instance of <see cref="ReplyMenu"/> with default data.
        /// </summary>
        public ReplyMenu() { }
        /// <summary>
        /// Creates a new instance of <see cref="ReplyMenu"/> with specified data.
        /// </summary>
        /// <param name="reply">Single button text.</param>
        public ReplyMenu(string reply) : this(new[] { reply }) { }
        /// <summary>
        /// Creates a new instance of <see cref="ReplyMenu"/> with specified data.
        /// </summary>
        /// <param name="replies">Several buttons' text.</param>
        public ReplyMenu(params string[] replies) : this(replies.ToList()) { }
        /// <summary>
        /// Creates a new instance of <see cref="ReplyMenu"/> with specified data.
        /// </summary>
        /// <param name="buttons">Several buttons' text.</param>
        public ReplyMenu(List<string> buttons) => Buttons = buttons;
        /// <summary>
        /// Adds new button to menu.
        /// </summary>
        /// <param name="reply">Button to add.</param>
        public void Add(string reply) => Buttons.Add(reply);
        /// <summary>
        /// Adds several buttons to menu.
        /// </summary>
        /// <param name="replies">Buttons to add.</param>
        public void AddRange(params string[] replies) => Buttons.AddRange(replies.ToList());
        /// <summary>
        /// Adds several buttons to menu.
        /// </summary>
        /// <param name="buttons">Buttons to add.</param>
        public void AddRange(List<string> buttons) => Buttons.AddRange(buttons);

        /// <summary>
        /// Creates specific <see cref="IReplyMarkup"/> that could be pushed to telegram's API.
        /// </summary>
        /// <returns>Converted to <see cref="IReplyMarkup"/> <see cref="IMesMenu"/>'s interior.</returns>
        public IReplyMarkup GetMarkup()
        {
            List<List<KeyboardButton>> data = new();
            int ti = 0;
            List<KeyboardButton> temp = new();
            for (int i = 0; i < Buttons.Count; i++)
            {
                var btn = Buttons[i];
                temp.Add(new KeyboardButton(btn));
                ti++;
                if (ti % ColumnsCount == 0)
                {
                    data.Add(temp);
                    temp = new();
                }
            }
            if (temp.Count != 0) data.Add(temp);

            return new ReplyKeyboardMarkup(data)
            {
                ResizeKeyboard = ResizeKeyboard,
                OneTimeKeyboard = OneTimeKeyboard,
            };
        }

        /// <summary>
        /// Combines <see cref="ReplyMenu"/> interiors.
        /// </summary>
        /// <param name="left">First menu.</param>
        /// <param name="right">Second menu.</param>
        /// <returns>New <see cref="ReplyMenu"/> that contains data from both: <paramref name="left"/> and <paramref name="right"/> menus.</returns>
        public static ReplyMenu operator+(ReplyMenu left, ReplyMenu right)
        {
            var buttons = new List<string>();
            buttons.AddRange(left.Buttons);
            buttons.AddRange(right.Buttons);
            return new ReplyMenu(buttons);
        }
    }
}