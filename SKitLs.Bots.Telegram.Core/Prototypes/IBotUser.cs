using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IBotUser : IBotDisplayable
    {
        public long TelegramId { get; }
        public int PermissionLevel { get; }
        public int StateId { get; }
    }
}
