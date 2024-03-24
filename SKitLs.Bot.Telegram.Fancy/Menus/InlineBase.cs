using SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Menus
{
    /// <summary>
    /// Abstract base class for representing custom <see href="https://core.telegram.org/bots/api#inlinekeyboardbutton">inline keyboards</see>.
    /// Implements the <see cref="IMessageMenu"/> interface to provide the mechanics of building the keyboard.
    /// </summary>
    public abstract class InlineBase : IBuildableContent<IMessageMenu>
    {
        private int _columnsCount = 1;

        /// <summary>
        /// Gets or sets the number of columns in the menu or keyboard layout.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when attempting to set a value less than 1.</exception>
        public int ColumnsCount
        {
            get => _columnsCount;
            set => _columnsCount = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(ColumnsCount));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineBase"/> class with the specified number of columns.
        /// </summary>
        /// <param name="columnsCount">The number of columns in the menu or keyboard layout.</param>
        public InlineBase(int columnsCount)
        {
            ColumnsCount = columnsCount;
        }

        /// <summary>
        /// Gets the list of menu buttons for the derived class.
        /// </summary>
        /// <returns>A list of <see cref="IInlineButton"/> representing the menu buttons.</returns>
        protected abstract List<IBuildableContent<IInlineButton>> GetButtons();

        /// <inheritdoc/>
        public abstract object Clone();

        /// <inheritdoc/>
        /// <remarks>
        /// If a button in the menu supports the <see cref="IBuildableContent{T}"/> interface, it will be automatically built.
        /// </remarks>
        public virtual async Task<IMessageMenu> BuildContentAsync(ICastedUpdate? update)
        {
            var buttons = GetButtons();
            List<List<InlineKeyboardButton>> data = [];
            int ti = 0;
            List<InlineKeyboardButton> temp = [];
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

            return new MenuWrapper(new InlineKeyboardMarkup(data));

            void SaveRow()
            {
                data.Add(temp);
                temp = [];
            }
        }
    }
}