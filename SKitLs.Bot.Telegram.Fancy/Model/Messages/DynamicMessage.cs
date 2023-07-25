using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    public class DynamicMessage : IOutputMessage, IDynamicMessage
    {
        public int ReplyToMessageId { get; set; }

        public ParseMode? ParseMode { get; set; }
        public IMesMenu? Menu { get; set; }

        public Func<ISignedUpdate?, IOutputMessage> MessageBuilder { get; set; }

        public DynamicMessage(Func<ISignedUpdate?, IOutputMessage> builder) => MessageBuilder = builder;
        public DynamicMessage(DynamicMessage other) => MessageBuilder = other.MessageBuilder;

        public object Clone() => new DynamicMessage(this);

        public IOutputMessage BuildWith(ISignedUpdate? update) => MessageBuilder(update);
        public string GetMessageText() => MessageBuilder(null).GetMessageText();
    }
}