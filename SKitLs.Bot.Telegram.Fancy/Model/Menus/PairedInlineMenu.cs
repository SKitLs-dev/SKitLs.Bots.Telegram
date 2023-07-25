using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    /// <summary>
    /// Specific menu that realizes the inline one. Displayed as inline message's menu.
    /// </summary>
    public class PairedInlineMenu : IMesMenu
    {
        /// <summary>
        /// Defines menu's columns count.
        /// </summary>
        public int ColumnsCount { get; set; } = 1;
        /// <summary>
        /// Internal container that stores temporary callbacks data to set it up in finalization.
        /// </summary>
        private readonly List<InlineButtonPair> menus = new();
        /// <summary>
        /// Serializer used to simplify addition process.
        /// </summary>
        public IArgsSerilalizerService? Serializer { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="PairedInlineMenu"/> with default data.
        /// </summary>
        public PairedInlineMenu() { }
        /// <summary>
        /// Creates a new instance of <see cref="ReplyMenu"/> with specified data.
        /// </summary>
        /// <param name="label">Callback's label.</param>
        /// <param name="data">Callback's data.</param>
        public PairedInlineMenu(string label, string data) => Add(label, data);

        /// <summary>
        /// Adds new default callback.
        /// </summary>
        /// <param name="callback">Callback to add.</param>
        /// <param name="singleLine">Determines whether button should be placed on a single line.</param>
        public void Add(DefaultCallback callback, bool singleLine = false)
            => Add(callback.Label, callback.GetSerializedData(), singleLine);
        /// <summary>
        /// Adds new argument callback.
        /// </summary>
        /// <typeparam name="T">Represents action's argument type.</typeparam>
        /// <param name="callback">Callback to add.</param>
        /// <param name="data">Argument to be implemented.</param>
        /// <param name="singleLine">Determines whether button should be placed on a single line.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add<T>(BotArgedCallback<T> callback, T data, bool singleLine = false)
            where T : notnull, new() => Add(callback.Label, callback.GetSerializedData(data, Serializer ?? throw new ArgumentNullException(nameof(Serializer))), singleLine);
        /// <summary>
        /// Adds custom bot action.
        /// </summary>
        /// <param name="customLabel">Action's custom label.</param>
        /// <param name="callback">Action to add.</param>
        /// <param name="singleLine">Determines whether button should be placed on a single line.</param>
        public void Add(string customLabel, IBotAction<SignedCallbackUpdate> callback, bool singleLine = false)
            => Add(customLabel, callback.GetSerializedData(), singleLine);
        /// <summary>
        /// Adds custom argument action.
        /// </summary>
        /// <typeparam name="T">Represents action's argument type.</typeparam>
        /// <param name="customLabel">Action's custom label.</param>
        /// <param name="callback">Action to add.</param>
        /// <param name="data">Argument to be implemented.</param>
        /// <param name="singleLine">Determines whether button should be placed on a single line.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add<T>(string customLabel, IArgedAction<T,SignedCallbackUpdate> callback, T data, bool singleLine = false)
            where T : notnull, new() => Add(customLabel, callback.GetSerializedData(data, Serializer ?? throw new ArgumentNullException(nameof(Serializer))), singleLine);
        /// <summary>
        /// Adds new labeled pair.
        /// </summary>
        /// <param name="data">Callback's data pair.</param>
        /// <param name="singleLine">Determines whether button should be placed on a single line.</param>
        public void Add(LabeledData data, bool singleLine = false)
            => Add(data.Label, data.Data, singleLine);
        /// <summary>
        /// Adds new data pair.
        /// </summary>
        /// <param name="label">Callback's label.</param>
        /// <param name="data">Callback's data.</param>
        /// <param name="singleLine">Determines whether button should be placed on a single line.</param>
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