using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.PageNavs.Prototype
{
    public interface IPageMenu
    {
        // Try: Build(IInlineBuilable)
        public IMesMenu Build(IPageWrap? previous, IPageWrap owner);
    }
}