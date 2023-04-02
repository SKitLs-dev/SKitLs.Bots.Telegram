//using SKitLs.Bots.Telegram.Core.CastedUpdates;
//using SKitLs.Bots.Telegram.Core.UpdateHandlers;
//using SKitLs.TGBots.Model.Bot.UpdateHandlers.MessgeUpdates;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SKitLs.TGBots.Model.Bot.UpdateHandlers
//{
//    internal class DefaultMessageUpdateHandler : IAnonMessageUpdateHandler
//    {
//        public ITextMessageUpdateHandler TextMessageUpdateHandler { get; set; }
//        public void UseCustomTextMessageUpdateHandler(ITextMessageUpdateHandler handler)
//            => TextMessageUpdateHandler = handler;

//        public DefaultMessageUpdateHandler()
//        {
//            TextMessageUpdateHandler = new DefaultTextMessageUpdateHandler();
//        }

//        public async Task HandleUpdateAsync(MessageUpdate update)
//        {
//            //if (update.Trigger.Type == MessageType.Text && TextMessageUpdateHandler != null)
//            //{
//            //    await TextMessageUpdateHandler.HandleUpdateAsync(new SignedMessageTextUpdate(update));
//            //}
//        }
//    }
//}
