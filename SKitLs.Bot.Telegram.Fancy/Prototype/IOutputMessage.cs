using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    public interface IOutputMessage : IBuildableMessage
    {
        public int ReplyToMessageId { get; }

        public ParseMode? ParseMode { get; set; }
        public IMesMenu? Menu { get; set; }
    }
}