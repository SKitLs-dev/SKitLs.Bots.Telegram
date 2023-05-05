using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem
{
    public interface IDelieveryService
    {
        public BotManager Owner { get; }

        public bool IsParseSafe(ParseMode mode, string part);
        public string MakeParseSafe(ParseMode mode, string part);

        public Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts = null);
        public Task<DelieveryResponse> SendMessageToChatAsync(IOutputMessage message, long chatId, CancellationTokenSource? cts = null);
        public Task<DelieveryResponse> ReplyToSender(string message, ISignedUpdate update, CancellationTokenSource? cts = null);
        public Task<DelieveryResponse> ReplyToSender(IOutputMessage message, ISignedUpdate update, CancellationTokenSource? cts = null);
    }
}