using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Reply
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
        /// Initializes a new instance of the <see cref="ReplyButton"/> class with the specified label text and optional settings.
        /// </summary>
        /// <param name="label">The text displayed on the button.</param>
        /// <param name="singleLine">Specifies whether the button should be displayed on a single line.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided label is null.</exception>
        public ReplyButton(string label, bool singleLine = false)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
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

        /// <inheritdoc/>
        public virtual object Clone() => new ReplyButton(Label, SingleLine);
    }
}