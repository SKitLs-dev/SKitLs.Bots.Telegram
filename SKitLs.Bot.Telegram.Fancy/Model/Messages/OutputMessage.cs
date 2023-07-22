using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    public abstract class OutputMessage : IOutputMessage, IFormattableMessage
    {
        public int ReplyToMessageId { get; set; }
        public ParseMode? ParseMode { get; set; }
        public IMesMenu? Menu { get; set; }

        public Func<IOutputMessage, ISignedUpdate, IOutputMessage>? FormattedClone { get; set; }

        public OutputMessage() { }
        public OutputMessage(IOutputMessage other)
        {
            ReplyToMessageId = other.ReplyToMessageId;
            Menu = other.Menu;
            ParseMode = other.ParseMode;
            if (other is IFormattableMessage formattable)
                FormattedClone = formattable.FormattedClone;
        }

        public OutputMessage UseParseMode(ParseMode mode)
        {
            ParseMode = mode;
            return this;
        }
        public OutputMessage AddMenu(IMesMenu? menu)
        {
            Menu = menu;
            return this;
        }

        public abstract string GetMessageText();

        // TODO
        public abstract object Clone();
    }
}
