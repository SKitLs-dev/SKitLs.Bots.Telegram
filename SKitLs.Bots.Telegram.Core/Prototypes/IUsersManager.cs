using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public delegate Task UserDataChanged(IBotUser user);

    public interface IUsersManager
    {
        public UserDataChanged? SignedEventHandled { get; }

        public Task<bool> IsUserRegistered(long telegramId);
        public Task<IBotUser?> GetUserById(long telegramId);
        public Task<IBotUser?> RegisterNewUser(ICastedUpdate update);
    }
}