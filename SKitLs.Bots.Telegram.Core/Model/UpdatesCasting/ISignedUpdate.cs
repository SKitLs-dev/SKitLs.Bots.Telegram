using SKitLs.Bots.Telegram.Core.Model.Delievery;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public interface ISignedUpdate
    {
        public ITelegramBotClient Bot { get; }
        public IBotUser Sender { get; }

        public Task<DelieveryResponse> SendMessageToSender(string message, CancellationTokenSource? cts);
    }
}
