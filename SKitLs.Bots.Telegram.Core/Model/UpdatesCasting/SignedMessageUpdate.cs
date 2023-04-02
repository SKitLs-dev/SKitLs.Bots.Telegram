using SKitLs.Bots.Telegram.Core.external.Loggers;
using SKitLs.Bots.Telegram.Core.Model.Delievery;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public class SignedMessageUpdate : MessageUpdate, ISignedUpdate
    {
        public int TriggerMessageId { get; set; }
        public IBotUser Sender { get; set; }

        public SignedMessageUpdate(Update trigger, ITelegramBotClient bot, long chatId, ChatType chatType,
            IBotUser sender, ILogger logger)
            : base(trigger, bot, chatId, chatType, logger)
        {
            if (trigger.Type == UpdateType.Message && trigger.Message != null)
            {
                Message = trigger.Message;
                TriggerMessageId = Message.MessageId;
            }
            else
                throw new ArgumentNullException(nameof(trigger), "Не удалось получить сообщение из обновления");

            Sender = sender;
        }
        public SignedMessageUpdate(CastedChatUpdate update, IBotUser sender)
            : this(update.OriginalSource, update.Bot, update.ChatId, update.ChatType, sender, update.Logger)
        { }
        protected SignedMessageUpdate(SignedMessageUpdate update)
            : this(update.OriginalSource, update.Bot, update.ChatId, update.ChatType, update.Sender, update.Logger)
        { }
        public Task<DelieveryResponse> SendMessageToSender(string message, CancellationTokenSource? cts = null)
            => SendMessageToChatAsync(message, Sender.TelegramId, cts);
    }
}
