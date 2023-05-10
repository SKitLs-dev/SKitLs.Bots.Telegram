using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    public interface IOutputMessage : IBuildableMessage
    {
        public ParseMode? ParseMode { get; set; }
        public IReplyMarkup? Markup { get; set; }
    }
}