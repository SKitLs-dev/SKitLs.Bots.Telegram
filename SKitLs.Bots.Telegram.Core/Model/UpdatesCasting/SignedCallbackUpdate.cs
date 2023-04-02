using SKitLs.Bots.Telegram.Core.external.Loggers;
using SKitLs.Bots.Telegram.Core.Model.Delievery;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public class SignedCallbackUpdate : CastedChatUpdate, ISignedUpdate
    {
        public CallbackQuery Trigger { get; set; }
        public Message Message { get; set; }
        public int TriggerMessageId { get; set; }
        public IBotUser Sender { get; set; }
        public string Data { get; set; }

        public SignedCallbackUpdate(Update trigger, ITelegramBotClient bot, long chatId, ChatType chatType,
            IBotUser sender, ILogger logger)
            : base(trigger, bot, chatId, chatType, logger)
        {
            if (trigger.Type == UpdateType.CallbackQuery
                && trigger.CallbackQuery != null
                && trigger.CallbackQuery.Data != null
                && trigger.CallbackQuery.Message != null)
            {
                Trigger = trigger.CallbackQuery;
                Message = trigger.CallbackQuery.Message;
                TriggerMessageId = trigger.CallbackQuery.Message.MessageId;
                Data = trigger.CallbackQuery.Data;
                Sender = sender;
            }
            else
                throw new ArgumentNullException(nameof(trigger), "Не удалось получить сообщение из обновления");
        }
        public SignedCallbackUpdate(CastedChatUpdate update, IBotUser sender)
            : this(update.OriginalSource, update.Bot, update.ChatId, update.ChatType, sender, update.Logger) { }
        
        public Task<DelieveryResponse> SendMessageToSender(string message, CancellationTokenSource? cts = null)
            => SendMessageToChatAsync(message, Sender.TelegramId, cts);
    }
}
