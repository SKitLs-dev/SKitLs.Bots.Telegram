//using SKitLs.Bots.Telegram.Core.CastedUpdates;
//using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
//using SKitLs.Bots.Telegram.Core.Prototypes;
//using SKitLs.Bots.Telegram.Core.UpdateHandlers;
//using SKitLs.TGBots.Model.Bot.UpdateHandlers.MessgeUpdates;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Schema;

//namespace SKitLs.TGBots.Model.Bot.UpdateHandlers
//{
//    internal class DefaultMessageUpdateHandler : IUpdateHandlerBase<AnonimMessageUpdate>
//    {
//        //public ITextMessageUpdateHandler TextMessageUpdateHandler { get; set; }
//        //public void UseCustomTextMessageUpdateHandler(ITextMessageUpdateHandler handler)
//        //    => TextMessageUpdateHandler = handler;

//        //public DefaultMessageUpdateHandler()
//        //{
//        //    TextMessageUpdateHandler = new DefaultTextMessageUpdateHandler();
//        //}

//        public async Task HandleUpdateAsync(AnonimMessageUpdate update)
//        {
//            //if (update.Trigger.Type == MessageType.Text && TextMessageUpdateHandler != null)
//            //{
//            //    await TextMessageUpdateHandler.HandleUpdateAsync(new SignedMessageTextUpdate(update));
//            //}
//        }

//        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
//        {
//            await HandleUpdateAsync(BuildUpdate(update, sender));
//        }

//        public AnonimMessageUpdate BuildUpdate(CastedUpdate update, IBotUser? sender)
//        {
//            return new AnonimMessageUpdate(update);
//        }
//    }
//}