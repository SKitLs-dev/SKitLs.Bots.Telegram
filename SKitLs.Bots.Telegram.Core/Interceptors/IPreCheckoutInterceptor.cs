using SKitLs.Bots.Telegram.Core.Building;
using Telegram.Bot.Types.Payments;

namespace SKitLs.Bots.Telegram.Core.Interceptors
{
    /// <summary>
    /// Represents an interface for interceptors handling pre-checkout queries.
    /// Implementing this interface allows classes to intercept and handle pre-checkout queries received by the bot.
    /// <para/>
    /// Supports: <see cref="IOwnerCompilable"/>
    /// </summary>
    public interface IPreCheckoutInterceptor : IUpdateInterceptor, IOwnerCompilable
    {
        /// <summary>
        /// Asynchronously handles the pre-checkout query asynchronously.
        /// </summary>
        /// <param name="preCheckout">The pre-checkout query to handle.</param>
        public Task HandlePreCheckoutAsync(PreCheckoutQuery preCheckout);
    }
}