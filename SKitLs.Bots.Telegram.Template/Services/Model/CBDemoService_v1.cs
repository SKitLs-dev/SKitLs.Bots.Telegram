using SKitLs.Bots.Telegram.Core.DeliverySystem;
using SKitLs.Bots.Telegram.Core.Services;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Template.Services.Prototype;

namespace SKitLs.Bots.Telegram.Template.Services.Model
{
    internal class CBDemoService_v1 : BotServiceBase, ICBDemoService
    {
        private IDeliveryService Delivery => Owner.DeliveryService;

        public async Task Run(SignedCallbackUpdate update)
        {
            await Delivery.SendMessageToChatAsync(update.ChatId, "v1 service reply");
        }
    }
}
