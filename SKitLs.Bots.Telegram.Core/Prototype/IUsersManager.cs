using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// Provides methods for advanced manipulation of user data with a default type of <see cref="IBotUser"/>.
    /// </summary>
    public interface IUsersManager : IUsersManager<IBotUser> { }

    /// <summary>
    /// Provides methods for advanced manipulation of user data, specifically designed for managing users of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of user data that this manager operates on, typically implementing <see cref="IBotUser"/>.</typeparam>
    public interface IUsersManager<T> where T : IBotUser
    {
        /// <summary>
        /// Occurs when a signed update is handled, providing user data for the sender.
        /// </summary>
        public UserDataChanged<T>? SignedUpdateHandled { get; set; }

        /// <summary>
        /// Asynchronously checks if a user with the specified <paramref name="telegramId"/> is registered.
        /// </summary>
        /// <param name="telegramId">The user's ID to check.</param>
        /// <returns><see langword="true"/> if user data is defined; otherwise, <see langword="false"/>.</returns>
        public Task<bool> CheckIfRegisteredAsync(long telegramId);

        /// <summary>
        /// Asynchronously retrieves user data for the specified <paramref name="telegramId"/>.
        /// </summary>
        /// <param name="telegramId">The user's ID to retrieve data for.</param>
        /// <returns>An instance of <typeparamref name="T"/> if the user is registered; otherwise, <see langword="null"/>.</returns>
        public Task<T?> GetUserByIdAsync(long telegramId);

        /// <summary>
        /// Asynchronously registers a new user using incoming <paramref name="update"/> data.
        /// </summary>
        /// <param name="update">The incoming update data used for user registration.</param>
        /// <returns>An instance of <typeparamref name="T"/> if the user is successfully registered; otherwise, <see langword="null"/>.</returns>
        public Task<T?> RegisterNewUserAsync(ICastedUpdate update);
    }
}