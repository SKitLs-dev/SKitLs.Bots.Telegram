using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Inline
{
    /// <summary>
    /// Represents a basic <see cref="IInlineButton"/> that can be included in an <see cref="InlineMenu"/>.
    /// </summary>
    public class InlineButton : IInlineButton, IBuildableContent<IInlineButton>
    {
        /// <inheritdoc/>
        public virtual string Label { get; set; }

        /// <inheritdoc/>
        public virtual string Data { get; set; }

        /// <inheritdoc/>
        public virtual bool SingleLine { get; set; }

        /// <summary>
        /// <b>Optional.</b> Represents the content builder for this inline button.
        /// </summary>
        public virtual ContentBuilder<InlineButton>? ContentBuilder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineButton"/> class with the specified label, data, and optional settings.
        /// </summary>
        /// <param name="label">The label text of the button.</param>
        /// <param name="data">The data to be sent in a callback query to the bot when the button is pressed.</param>
        /// <param name="singleLine"><b>Optional.</b> Indicates whether the button should be displayed in a single line.</param>
        public InlineButton(string label, string data, bool singleLine = false)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Data = data ?? throw new ArgumentNullException(nameof(data));
            SingleLine = singleLine;
        }

        /// <inheritdoc/>
        public virtual async Task<IInlineButton> BuildContentAsync(ICastedUpdate? update)
        {
            if (ContentBuilder is not null)
                return await ContentBuilder.Invoke(this, update);
            else return this;
        }

        /// <inheritdoc/>
        public virtual InlineKeyboardButton GetButton() => InlineKeyboardButton.WithCallbackData(Label, Data);

        /// <inheritdoc/>
        public virtual object Clone() => new InlineButton(Label, Data, SingleLine);
    }
}