using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    public class MenuCleaner : IMesMenu
    {
        public IReplyMarkup GetMarkup() => new ReplyKeyboardRemove();
    }
}