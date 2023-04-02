using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.Core.Model.DelieverySystem
{
    internal class DefaultDelieveryService
    {

        //public async Task<DelieveryResponse> SendMessageToChatAsync(string message, long chatId, CancellationTokenSource? cts)
        //{
        //    cts ??= new();
        //    try
        //    {
        //        await Bot.SendTextMessageAsync(
        //            chatId: chatId,
        //            text: message,
        //            cancellationToken: cts.Token);
        //        return DelieveryResponse.OK();
        //    }
        //    catch (Exception)
        //    {
        //        cts.Cancel();
        //        return DelieveryResponse.Forbidden();
        //    }
        //}
    }
}
