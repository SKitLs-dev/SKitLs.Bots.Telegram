using SKitLs.Bots.Telegram.Core.DeliverySystem;
using SKitLs.Bots.Telegram.Core.Services;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Template.Services.Prototype;

namespace SKitLs.Bots.Telegram.Template.Services.Model
{
    internal class CBDemoService_v2 : BotServiceBase, ICBDemoService
    {
        private IDeliveryService Delivery => Owner.DeliveryService;
        private string Reply { get; init; }

        public CBDemoService_v2(string reply) => Reply = reply;

        public async Task Run(SignedCallbackUpdate update)
        {
            await Delivery.AnswerSenderAsync($"v2 service reply: {Reply}", update);
        }
    }
}
