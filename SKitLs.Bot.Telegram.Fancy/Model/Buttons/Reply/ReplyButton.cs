using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Reply
{
    // TODO: add RequestChat and RequestWebApp buttons

    /// <summary>
    /// Represents a simple reply button with a text, implementing the <see cref="IReplyButton"/> interface.
    /// </summary>
    public class ReplyButton : IReplyButton, IBuildableContent<IReplyButton>
    {
        /// <inheritdoc/>
        public virtual string Label { get; set; }
        /// <inheritdoc/>
        public virtual bool SingleLine { get; set; }

        /// <summary>
        /// <b>Optional.</b> Represents the content builder for this inline button.
        /// </summary>
        public virtual ContentBuilder<ReplyButton>? ContentBuilder { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ReplyButton"/> class with the specified label and optional settings.
        /// </summary>
        /// <param name="label">The label text for the button.</param>
        /// <param name="singleLine">Indicates whether the button should be displayed in a single line.</param>
        public ReplyButton(string label, bool singleLine = false)
        {
            Label = label;
            SingleLine = singleLine;
        }

        /// <inheritdoc/>
        public virtual async Task<IReplyButton> BuildContentAsync(ICastedUpdate? update)
        {
            if (ContentBuilder is not null)
                return await ContentBuilder.Invoke(this, update);
            else return this;
        }

        /// <inheritdoc/>
        public virtual KeyboardButton GetButton() => new(Label);
    }
}