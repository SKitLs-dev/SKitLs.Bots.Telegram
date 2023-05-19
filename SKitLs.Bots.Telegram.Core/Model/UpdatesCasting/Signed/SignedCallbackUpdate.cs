using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    public class SignedCallbackUpdate : CastedUpdate, ISignedUpdate
    {
        public CallbackQuery Trigger { get; set; }
        public Message Message { get; set; }
        public int TriggerMessageId { get; set; }
        public IBotUser Sender { get; set; }
        public string Data { get; set; }

        public SignedCallbackUpdate(BotManager owner, Update source, long chatId, ChatType chatType, IBotUser sender)
            : base(owner, source, chatType, chatId)
        {
            if (source.CallbackQuery is null
                || source.CallbackQuery.Data is null
                || source.CallbackQuery.Message is null)
                throw new UpdateCastingException("Callback", source.Id);

            Trigger = source.CallbackQuery;
            Message = source.CallbackQuery.Message;
            TriggerMessageId = source.CallbackQuery.Message.MessageId;
            Data = source.CallbackQuery.Data;
            Sender = sender;
        }

        public SignedCallbackUpdate(CastedUpdate update, IBotUser sender)
            : this(update.Owner, update.OriginalSource, update.ChatId, update.ChatType, sender)
        { }
    }
}