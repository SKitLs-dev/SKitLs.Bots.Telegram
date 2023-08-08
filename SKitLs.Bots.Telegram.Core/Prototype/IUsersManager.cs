using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// Provides methods for advanced manipulation of users' data.
    /// </summary>
    public interface IUsersManager
    {
        /// <summary>
        /// Event that occurs when a signed update is handled, passing the user identified as the sender.
        /// </summary>
        public UserDataChanged? SignedUpdateHandled { get; }

        /// <summary>
        /// Asynchronously checks whether a user with the specified <paramref name="telegramId"/> is registered.
        /// </summary>
        /// <param name="telegramId">The user's ID.</param>
        /// <returns><see langword="true"/> if user data is defined, otherwise <see langword="false"/>.</returns>
        public Task<bool> IsUserRegisteredAsync(long telegramId);
        
        /// <summary>
        /// Asynchronously retrieves user data for the specified <paramref name="telegramId"/>.
        /// </summary>
        /// <param name="telegramId">The user's ID.</param>
        /// <returns>An instance of <see cref="IBotUser"/> if the user is registered; otherwise, <see langword="null"/>.</returns>
        public Task<IBotUser?> GetUserByIdAsync(long telegramId);
        
        /// <summary>
        /// Asynchronously registers a new user using incoming <paramref name="update"/> data.
        /// </summary>
        /// <param name="update">The incoming update data used for user registration.</param>
        /// <returns>An instance of <see cref="IBotUser"/> if the user is successfully registered; otherwise, <see langword="null"/>.</returns>
        public Task<IBotUser?> RegisterNewUserAsync(ICastedUpdate update);
    }
}