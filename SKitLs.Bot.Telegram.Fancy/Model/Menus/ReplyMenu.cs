using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    public class ReplyMenu : IMesMenu
    {
        public int ColumnsCount { get; set; } = 1;
        private List<string> Buttons { get; set; } = new();
        public bool ResizeKeyboard { get; set; } = true;
        public bool OneTimeKeyboard { get; set; } = true;

        public ReplyMenu() { }
        public ReplyMenu(string reply) => Buttons.Add(reply);
        public ReplyMenu(List<string> buttons) => Buttons = buttons;
        public void Add(string reply) => Buttons.Add(reply);

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
    }
}