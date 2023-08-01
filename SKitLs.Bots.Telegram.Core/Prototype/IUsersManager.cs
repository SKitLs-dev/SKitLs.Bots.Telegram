using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// Provides methods of advanced manipulations with users' data.
    /// </summary>
    public interface IUsersManager
    {
        /// <summary>
        /// Occurs when signed update is handled. Passes user, that has been determined as a sender.
        /// </summary>
        public UserDataChanged? SignedUpdateHandled { get; }

        /// <summary>
        /// Asynchronously checks if user with <paramref name="telegramId"/> is registered.
        /// </summary>
        /// <param name="telegramId">User's id.</param>
        /// <returns><see langword="true"/> if user's data is defined. Otherwise <see langword="false"/></returns>
        public Task<bool> IsUserRegisteredAsync(long telegramId);
        /// <summary>
        /// Asynchronously gets specified user data by its <paramref name="telegramId"/>.
        /// </summary>
        /// <param name="telegramId">User's id.</param>
        /// <returns><see cref="IBotUser"/> if user is registered, otherwise <see langword="null"/>.</returns>
        public Task<IBotUser?> GetUserByIdAsync(long telegramId);
        /// <summary>
        /// Asynchronously registries new user, using incoming <paramref name="update"/> data.
        /// </summary>
        /// <param name="update">.</param>
        /// <returns><see cref="IBotUser"/> if user is registered successfully, otherwise <see langword="null"/>.</returns>
        public Task<IBotUser?> RegisterNewUserAsync(ICastedUpdate update);
    }
}