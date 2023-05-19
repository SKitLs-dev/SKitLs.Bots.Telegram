using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    public abstract class OutputMessage : IOutputMessage
    {
        public int ReplyToMessageId { get; set; }
        public ParseMode? ParseMode { get; set; }
        public IMesMenu? Menu { get; set; }

        public Func<IOutputMessage, ISignedUpdate, IOutputMessage>? FormattedClone { get; set; }

        public OutputMessage() { }
        public OutputMessage(IOutputMessage other)
        {
            ReplyToMessageId = other.ReplyToMessageId;
            FormattedClone = other.FormattedClone;
            Menu = other.Menu;
            ParseMode = other.ParseMode;
        }

        public OutputMessage UseParseMode(ParseMode mode)
        {
            ParseMode = mode;
            return this;
        }
        public OutputMessage AddMenu(IMesMenu? markup)
        {
            Menu = markup;
            return this;
        }

        //public virtual string GetMessageText() => ToString() ?? GetType().Name;
        public abstract string GetMessageText();

        // TODO
        public abstract object Clone();
    }
}
