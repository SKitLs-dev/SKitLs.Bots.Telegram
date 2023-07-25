using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    /// <summary>
    /// Specific anti-menu that helps to remove <see cref="ReplyMenu"/> (<see cref="ReplyKeyboardMarkup"/>)
    /// </summary>
    public class ReplyCleaner : IMesMenu
    {
        public IReplyMarkup GetMarkup() => new ReplyKeyboardRemove();
    }
}