using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    public class PairedInlineMenu : IMesMenu
    {
        public int ColumnsCount { get; set; } = 1;
        private List<InlineButtonPair> menus = new();

        public PairedInlineMenu() { }
        public PairedInlineMenu(string label, string data) => Add(label, data);

        public void Add(string label, string data, bool singleLine = false)
            => menus.Add(new InlineButtonPair(label, data) { SingleLine = singleLine });

        public IReplyMarkup GetMarkup()
        {
            List<List<InlineKeyboardButton>> data = new();
            int ti = 0;
            List<InlineKeyboardButton> temp = new();
            for (int i = 0; i < menus.Count; i++)
            {
                var btn = menus[i];
                if (btn.SingleLine && temp.Count != 0)
                {
                    data.Add(temp);
                    temp = new();
                }
                temp.Add(InlineKeyboardButton.WithCallbackData(btn.Label, btn.Data));

                ti = btn.SingleLine ? 0 : ti + 1;
                if (ti % ColumnsCount == 0)
                {
                    data.Add(temp);
                    temp = new();
                }
            }
            if (temp.Count != 0) data.Add(temp);

            return new InlineKeyboardMarkup(data);
        }
    }
}