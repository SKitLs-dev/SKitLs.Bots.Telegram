using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    /// <summary>
    /// Abstract base class for representing custom <see href="https://core.telegram.org/bots/api#inlinekeyboardbutton">inline keyboards</see>.
    /// Implements the <see cref="IMessageMenu"/> interface to provide the mechanics of building the keyboard.
    /// </summary>
    public abstract class InlineBase : IBuildableContent<IMessageMenu>
    {
        /// <summary>
        /// Defines menu's columns count.
        /// </summary>
        public int ColumnsCount { get; set; } = 1;

        /// <summary>
        /// Gets the list of menu buttons for the derived class.
        /// </summary>
        /// <returns>A list of <see cref="IInlineButton"/> representing the menu buttons.</returns>
        protected abstract List<IInlineButton> GetButtons();

        /// <inheritdoc/>
        public abstract object Clone();

        /// <inheritdoc/>
        /// <remarks>
        /// If a button in the menu supports the <see cref="IBuildableContent{T}"/> interface, it will be automatically built.
        /// </remarks>
        public virtual async Task<IMessageMenu> BuildContentAsync(ICastedUpdate? update)
        {
            var buttons = GetButtons();
            List<List<InlineKeyboardButton>> data = new();
            int ti = 0;
            List<InlineKeyboardButton> temp = new();
            for (int i = 0; i < buttons.Count; i++)
            {
                // Get button
                var button = buttons[i] is IBuildableContent<IInlineButton> buildable ? await buildable.BuildContentAsync(update) : buttons[i];

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
                temp.Clear();
            }
        }
    }
}