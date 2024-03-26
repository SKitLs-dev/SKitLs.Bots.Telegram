using SKitLs.Bots.Telegram.AdvancedMessages.Messages;
using SKitLs.Bots.Telegram.Core.DeliverySystem;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    public static class DeliveryExtensions
    {
        public static async Task<DeliveryResponse> AnswerAsync(this ITelegramMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await update.Owner.DeliveryService.AnswerSenderAsync(message, update, cts ?? new());

        public static async Task<DeliveryResponse> AnswerAsync(this IBuildableMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await update.Owner.DeliveryService.AnswerSenderAsync(message, update, cts ?? new());

        public static async Task<DeliveryResponse> AnswerAsync(this IOutputMessage message, ISignedUpdate update, CancellationTokenSource? cts = null)
            => await update.Owner.DeliveryService.AnswerSenderAsync(message, update, cts ?? new());
    }
}