using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.Interceptors
{
    /// <summary>
    /// Represents an interface for update interceptors.
    /// </summary>
    public interface IUpdateInterceptor
    {
        /// <summary>
        /// Determines whether the interceptor should intercept the given update.
        /// </summary>
        /// <param name="update">The update to be intercepted.</param>
        /// <returns><see langword="true"/> if the interceptor should intercept the update; otherwise, <see langword="false"/>.</returns>
        public bool ShouldIntercept(Update update);

        /// <summary>
        /// Handles the intercepted update.
        /// </summary>
        /// <param name="update">The intercepted update.</param>
        /// <returns><see langword="true"/> if the update was intercepted and further processing should be terminated; otherwise, <see langword="false"/>.</returns>
        public bool HandleUpdate(Update update);
    }
}
