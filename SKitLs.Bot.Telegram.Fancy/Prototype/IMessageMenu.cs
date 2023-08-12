using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    /// <summary>
    /// Represents an interface that defines the basic mechanics for creating message menus.
    /// </summary>
    public interface IMessageMenu : ICloneable
    {
        /// <summary>
        /// Gets the reply markup for the message menu, which defines the custom keyboard or other user interface elements.
        /// </summary>
        /// <returns>The <see cref="IReplyMarkup"/> for the message menu, or null if no custom markup is provided.</returns>
        public IReplyMarkup? GetMarkup();
    }
}