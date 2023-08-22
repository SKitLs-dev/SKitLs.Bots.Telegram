using SKitLs.Bots.Telegram.Core.Model.Building;
using Telegram.Bot.Types.Payments;

namespace SKitLs.Bots.Telegram.Core.Model.Services
{
    public interface IPreCheckoutService : IOwnerCompilable
    {
        public Task HandlePreCheckoutAsync(PreCheckoutQuery preCheckout);
    }
}