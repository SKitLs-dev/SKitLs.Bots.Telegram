using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    public class PairedInlineMenu : IMesMenu
    {
        public int ColumnsCount { get; set; } = 1;
        private readonly List<InlineButtonPair> menus = new();
        public IArgsSerilalizerService? Serializer { get; set; }

        public PairedInlineMenu() { }
        public PairedInlineMenu(string label, string data) => Add(label, data);

        public void Add(DefaultCallback callback, bool singleLine = false)
            => Add(callback.Label, callback.GetSerializedData(), singleLine);
        public void Add<T>(BotArgedCallback<T> callback, T data, bool singleLine = false)
            where T : notnull, new() => Add(callback.Label, callback.GetSerializedData(data, Serializer ?? throw new ArgumentNullException(nameof(Serializer))), singleLine);
        public void Add(string customLabel, IBotAction<SignedCallbackUpdate> callback, bool singleLine = false)
            => Add(customLabel, callback.GetSerializedData(), singleLine);
        public void Add<T>(string customLabel, IArgedAction<T,SignedCallbackUpdate> callback, T data, bool singleLine = false)
            where T : notnull, new() => Add(customLabel, callback.GetSerializedData(data, Serializer ?? throw new ArgumentNullException(nameof(Serializer))), singleLine);
        public void Add(LabeledData data, bool singleLine = false)
            => Add(data.Label, data.Data, singleLine);
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