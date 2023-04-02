using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    public class SignedMessageUpdate : AnonimMessageUpdate, ISignedUpdate
    {
        public int TriggerMessageId { get; set; }
        public IBotUser Sender { get; set; }

        public SignedMessageUpdate(Update source, ChatType chatType, long chatId, IBotUser sender)
            : base(source, chatType, chatId)
        {
            if (source.Message is null)
                throw new UpdateCastingException("Signed Message", source.Id);

            Message = source.Message;
            TriggerMessageId = Message.MessageId;
            Sender = sender;
        }
        public SignedMessageUpdate(CastedUpdate update, IBotUser sender)
            : this(update.OriginalSource, update.ChatType, update.ChatId, sender)
        { }
    }
}
