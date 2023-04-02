//using SKitLs.Bots.Telegram.Core.Exceptions;
//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.MessageUpdates;
//using SKitLs.Bots.Telegram.Core.Prototypes;
//using SKitLs.Bots.Telegram.Management.AdvancedHandlers.Prototype;
//using Telegram.Bot.Types.Enums;

//namespace SKitLs.Bots.Telegram.Management.AdvancedHandlers.Model
//{
//    public class DefaultMessageUpdateHandler : IAnonMessageUpdateHandler
//    {
//        public ITextMessageUpdateHandler? TextMessageUpdateHandler { get; set; }
//        public void UseCustomTextMessageUpdateHandler(ITextMessageUpdateHandler handler)
//            => TextMessageUpdateHandler = handler;

//        public DefaultMessageUpdateHandler()
//        {
//            TextMessageUpdateHandler = new DefaultTextMessageUpdateHandler();
//        }

//        public async Task HandleUpdateAsync(CastedChatUpdate update, IBotUser? sender)
//        {
//            if (sender is null)
//                throw new NullSenderException();
            
//            await HandleUpdateAsync(new(update, sender));
//        }
//        public async Task HandleUpdateAsync(MessageUpdate update)
//        {
//            if (update.Message.Type == MessageType.Text && TextMessageUpdateHandler != null)
//            {
//                await TextMessageUpdateHandler.HandleUpdateAsync(new SignedMessageTextUpdate(update));
//            }
//        }
//    }
//}