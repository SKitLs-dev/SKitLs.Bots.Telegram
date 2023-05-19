using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    public interface IOutputMessage : IBuildableMessage
    {
        public int ReplyToMessageId { get; }

        public Func<IOutputMessage, ISignedUpdate, IOutputMessage>? FormattedClone { get; }
        public bool ShouldBeFormatted => FormattedClone != null;

        public ParseMode? ParseMode { get; set; }
        public IMesMenu? Menu { get; set; }
    }
}