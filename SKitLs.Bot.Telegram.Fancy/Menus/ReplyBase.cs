using SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Reply;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Menus
{
    /// <summary>
    /// Abstract base class for representing custom <see href="https://core.telegram.org/bots/api#replykeyboardmarkup">reply keyboards</see>
    /// with reply options.
    /// Implements the <see cref="IMessageMenu"/> interface to provide the mechanics of building the keyboard.
    /// </summary>
    public abstract class ReplyBase : IBuildableContent<IMessageMenu>
    {
        private int _columnsCount = 1;

        /// <summary>
        /// Gets or sets the number of columns in the menu or keyboard layout.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when attempting to set a value less than 1.</exception>
        public int ColumnsCount
        {
            // TODO Exception
            get => _columnsCount;
            set => _columnsCount = value > 0 ? value : throw new ArgumentOutOfRangeException($"Prop: {nameof(ColumnsCount)}; Value: {value}");
        }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#replykeyboardmarkup">Telegram API</see>]</b>
        /// <para/>
        /// Requests clients to always show the keyboard when the regular keyboard is hidden.
        /// Defaults to <see langword="false"/>, in which case the custom keyboard can be hidden and opened with a keyboard icon.
        /// </summary>
        public virtual bool IsPersistent { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#replykeyboardmarkup">Telegram API</see>]</b>
        /// <para/>
        /// Requests clients to resize the keyboard vertically for optimal fit
        /// (e.g., make the keyboard smaller if there are just two rows of buttons).
        /// Defaults to <see langword="true"/>, in which case the custom keyboard is always of the smaller as the app's standard keyboard.
        /// </summary>
        public virtual bool ResizeKeyboard { get; set; } = true;

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#replykeyboardmarkup">Telegram API</see>]</b>
        /// <para/>
        /// Requests clients to hide the keyboard as soon as it's been used.
        /// The keyboard will still be available, but clients will automatically display the usual letter-keyboard in the chat -
        /// the user can press a special button in the input field to see the custom keyboard again.
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public virtual bool OneTimeKeyboard { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#replykeyboardmarkup">Telegram API</see>]</b>
        /// <para/>
        /// The placeholder to be shown in the input field when the keyboard is active. Must be 1-64 characters.
        /// </summary>
        public string? InputFieldPlaceholder { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#replykeyboardmarkup">Telegram API</see>]</b>
        /// <para/>
        /// Use this parameter if you want to show the keyboard to specific users only.
        /// Targets:
        /// <list type="number">
        /// <item>Users that are @mentioned in the text of the Message object</item>
        /// <item>If the bot's message is a reply (has <see href="https://core.telegram.org/bots/api#sendmessage">reply_to_message_id</see>),
        /// sender of the original message</item>
        /// </list>
        /// <para/>
        /// Example: A user requests to change the bot's language, bot replies to the request with a keyboard to select the new language.
        /// Other users in the group don't see the keyboard.
        /// </summary>
        public bool Selective { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyBase"/> class with the specified number of columns.
        /// </summary>
        /// <param name="columnsCount">The number of columns in the menu or keyboard layout.</param>
        public ReplyBase(int columnsCount)
        {
            ColumnsCount = columnsCount;
        }

        /// <summary>
        /// Gets the list of menu buttons for the derived class.
        /// </summary>
        /// <returns>A list of <see cref="IReplyButton"/> representing the menu buttons.</returns>
        protected abstract List<IBuildableContent<IReplyButton>> GetButtons();

        /// <inheritdoc/>
        public abstract object Clone();

        /// <inheritdoc/>
        /// <remarks>
        /// If a button in the menu supports the <see cref="IBuildableContent{T}"/> interface, it will be automatically built.
        /// </remarks>
        public virtual async Task<IMessageMenu> BuildContentAsync(ICastedUpdate? update)
        {
            var buttons = GetButtons();
            List<List<KeyboardButton>> data = [];
            int ti = 0;
            List<KeyboardButton> temp = [];
            for (int i = 0; i < buttons.Count; i++)
            {
                // Get button
                var button = await buttons[i].BuildContentAsync(update);

                // If single line - append previous; set ti to -1 => ti++ = 0 => % ColCnt = 0 => saved
                if (button.SingleLine)
                {
                    SaveRow();
                    ti = -1;
                }

                ti++;
                temp.Add(button.GetButton());
                if (ti % ColumnsCount == 0)
                    SaveRow();
            }
            if (temp.Count != 0)
                SaveRow();

            var res = new ReplyKeyboardMarkup(data)
            {
                IsPersistent = IsPersistent,
                ResizeKeyboard = ResizeKeyboard,
                OneTimeKeyboard = OneTimeKeyboard,
                InputFieldPlaceholder = InputFieldPlaceholder,
                Selective = Selective,
            };
            return new MenuWrapper(res);

            void SaveRow()
            {
                data.Add(temp);
                temp = [];
            }
        }
    }
}