using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IDelieveryService
    {
        public Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts);
    }
}