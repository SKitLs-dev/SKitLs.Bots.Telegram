using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    /// <summary>
    /// Specific anti-menu that helps to remove <see cref="ReplyMenu"/> (<see cref="ReplyKeyboardRemove"/>)
    /// </summary>
    public class ReplyCleaner : IMesMenu
    {
        /// <summary>
        /// Creates specific <see cref="IReplyMarkup"/> that could be pushed to telegram's API.
        /// </summary>
        /// <returns>Converted to <see cref="IReplyMarkup"/> <see cref="IMesMenu"/>'s interior.</returns>
        public IReplyMarkup GetMarkup() => new ReplyKeyboardRemove();
    }
}