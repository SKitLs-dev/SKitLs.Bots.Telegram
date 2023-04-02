using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public class DefaultBotUser : IBotUser
    {
        public long Id => TelegramId;
        public long TelegramId { get; set; }
        public int PermissionLevel { get; private set; }
        public string DisplayName { get; set; }
        public int StateId => 0;

        public DefaultBotUser(long id, int permissionLevel = -1)
        {
            TelegramId = id;
            PermissionLevel = permissionLevel;
            DisplayName = "name";
        }

        public string ShortDisplay() => Id.ToString();
        public string FullDisplay(int fullness) => ShortDisplay();
        public string ListDisplay() => $"Неопознанный - {Id}";
    }
}
