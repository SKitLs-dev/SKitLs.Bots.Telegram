using SKitLs.Bots.Telegram.Core.external.Loggers;
using SKitLs.Bots.Telegram.Core.Model.Delievery;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public class CastedChatUpdate : CastedUpdate
    {
        public ChatType ChatType { get; set; }
        public long ChatId { get; set; }

        public CastedChatUpdate(Update trigger, ITelegramBotClient bot, long chatId, ChatType chatType, ILogger logger)
            : base(trigger, bot, logger)
        {
            ChatType = chatType;
            ChatId = chatId;
        }
        public async Task<DelieveryResponse> SendMessageTriggerToChatAsync(string message, CancellationTokenSource? cts = null)
            => await SendMessageToChatAsync(message, ChatId, cts);
    }
}
