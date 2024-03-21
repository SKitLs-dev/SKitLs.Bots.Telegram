using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Template.Services.Prototype;

namespace SKitLs.Bots.Telegram.Template.Services.Model
{
    internal class CBDemoService_v1 : ICBDemoService
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        private IDeliveryService Delivery => Owner.DeliveryService;

        public async Task Run(SignedCallbackUpdate update)
        {
            await Delivery.SendMessageToChatAsync(update.ChatId, "v1 service reply");
        }
    }
}
