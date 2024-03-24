using SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Reply;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    /// <summary>
    /// Provides methods for creating localized buttons.
    /// </summary>
    public static class Localize
    {
        /// <summary>
        /// Creates a localized inline button.
        /// </summary>
        /// <typeparam name="TButton">The type of the inline button.</typeparam>
        /// <param name="button">The inline button to localize.</param>
        /// <param name="format">Optional format arguments used for formatting the localized text.</param>
        /// <returns>A localized inline button.</returns>
        public static Localized<InlineButton, TButton, IInlineButton> Inline<TButton>(TButton button, params string?[] format) where TButton : InlineButton
        {
            return new Localized<InlineButton, TButton, IInlineButton>(button, nameof(InlineButton.Label), format);
        }

        /// <summary>
        /// Creates a localized reply button.
        /// </summary>
        /// <typeparam name="TButton">The type of the reply button.</typeparam>
        /// <param name="button">The reply button to localize.</param>
        /// <param name="format">Optional format arguments used for formatting the localized text.</param>
        /// <returns>A localized reply button.</returns>
        public static Localized<ReplyButton, TButton, IReplyButton> Reply<TButton>(TButton button, params string?[] format) where TButton : ReplyButton
        {
            return new Localized<ReplyButton, TButton, IReplyButton>(button, nameof(InlineButton.Label), format);
        }
    }
}