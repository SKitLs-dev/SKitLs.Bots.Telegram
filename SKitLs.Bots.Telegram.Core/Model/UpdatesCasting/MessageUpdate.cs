using SKitLs.Bots.Telegram.Core.external.Loggers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public class MessageUpdate : CastedChatUpdate
    {
        public Message Message { get; set; }

        public MessageUpdate(Update trigger, ITelegramBotClient bot, long chatId, ChatType chatType, ILogger logger)
            : base(trigger, bot, chatId, chatType, logger)
        {
            if (trigger.Type == UpdateType.Message && trigger.Message != null)
                Message = trigger.Message;
            else
                throw new ArgumentNullException(nameof(trigger), "Не удалось получить сообщение из обновления");
        }
        public MessageUpdate(CastedChatUpdate update)
            : this(update.OriginalSource, update.Bot, update.ChatId, update.ChatType, update.Logger)
        { }
    }
}
