using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Users;

namespace SKitLs.Bots.Telegram.Template.App
{
    internal class UsersManager : IUsersManager
    {
        public UserDataChanged<IBotUser>? SignedUpdateHandled { get; set; }

        private readonly List<DefaultBotUser> _users = [];

        public async Task<bool> CheckIfRegisteredAsync(long telegramId) => await GetUserByIdAsync(telegramId) is not null;

        public async Task<IBotUser?> GetUserByIdAsync(long telegramId) => await Task.FromResult(_users.Find(x => x.TelegramId == telegramId));

        public async Task<IBotUser?> RegisterNewUserAsync(ICastedUpdate update)
        {
            var user = TelegramHelper.GetSender(update.OriginalSource, this)!;
            var @new = new DefaultBotUser(user.Id, user.IsPremium.GetValueOrDefault(), user.LanguageCode ?? "en", user.FirstName);
            _users.Add(@new);
            return await Task.FromResult(@new);
        }
    }
}
