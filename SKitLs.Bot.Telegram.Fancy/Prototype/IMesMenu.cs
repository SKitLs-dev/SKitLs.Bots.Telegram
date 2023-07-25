using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    /// <summary>
    /// Provides basic mechanics of creating message's menus.
    /// </summary>
    public interface IMesMenu
    {
        /// <summary>
        /// Creates specific <see cref="IReplyMarkup"/> that could be pushed to telegram's API.
        /// </summary>
        /// <returns>Converted to <see cref="IReplyMarkup"/> <see cref="IMesMenu"/>'s interior.</returns>
        public IReplyMarkup GetMarkup();
    }
}