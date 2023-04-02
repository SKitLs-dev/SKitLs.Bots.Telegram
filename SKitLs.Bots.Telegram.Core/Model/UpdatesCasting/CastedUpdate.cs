using Microsoft.VisualBasic;
using SKitLs.Bots.Telegram.Core.external.Loggers;
using SKitLs.Bots.Telegram.Core.Model.Delievery;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public class CastedUpdate
    {
        public ILogger Logger { get; set; }
        public Update OriginalSource { get; set; }
        public UpdateType Type => OriginalSource.Type;
        public ITelegramBotClient Bot { get; set; }

        public CastedUpdate(Update trigger, ITelegramBotClient bot, ILogger logger)
        {
            OriginalSource = trigger;
            Bot = bot;
            Logger = logger;
        }

        public async Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts)
        {
            cts ??= new();
            try
            {
                await Bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: message,
                    cancellationToken: cts.Token);
                return DelieveryResponse.OK();
            }
            catch (Exception)
            {
                cts.Cancel();
                return DelieveryResponse.Forbidden();
            }
        }
    }
}
