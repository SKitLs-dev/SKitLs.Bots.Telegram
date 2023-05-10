using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem
{
    public interface IDelieveryService : IOwnerCompilable
    {
        public bool IsParseSafe(ParseMode mode, string part);
        public string MakeParseSafe(ParseMode mode, string part);

        public Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts = null);
        public Task<DelieveryResponse> SendMessageToChatAsync(IBuildableMessage message, long chatId, CancellationTokenSource? cts = null);
        
        public Task<DelieveryResponse> ReplyToSender(string message, ISignedUpdate update, CancellationTokenSource? cts = null);
        public Task<DelieveryResponse> ReplyToSender(IBuildableMessage message, ISignedUpdate update, CancellationTokenSource? cts = null);
    }
}