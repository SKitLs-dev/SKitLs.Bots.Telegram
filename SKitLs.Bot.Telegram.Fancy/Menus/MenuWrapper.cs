using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Menus
{
    /// <summary>
    /// Represents a wrapper for a custom message menu (reply markup) in a Telegram bot.
    /// <para/>
    /// <see cref="MenuWrapper"/> serves the purpose of decoupling the logic provided by <see cref="IMessageMenu"/>
    /// from the <see cref="IMessageMenu"/> logic.
    /// </summary>
    public class MenuWrapper : IMessageMenu
    {
        /// <summary>
        /// Gets or sets the underlying reply markup associated with this menu wrapper.
        /// </summary>
        public IReplyMarkup Markup { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuWrapper"/> class with the specified reply markup.
        /// </summary>
        /// <param name="markup">The reply markup to be wrapped by this menu.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided markup is null.</exception>
        public MenuWrapper(IReplyMarkup markup)
        {
            Markup = markup ?? throw new ArgumentNullException(nameof(markup));
        }

        /// <summary>
        /// Gets the wrapped reply markup.
        /// </summary>
        /// <returns>The underlying reply markup associated with this menu wrapper.</returns>
        public IReplyMarkup GetMarkup() => Markup;

        /// <inheritdoc/>
        public object Clone() => new MenuWrapper(Markup);
    }
}