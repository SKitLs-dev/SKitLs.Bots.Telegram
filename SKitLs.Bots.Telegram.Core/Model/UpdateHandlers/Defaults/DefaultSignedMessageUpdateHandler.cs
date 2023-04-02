//using SKitLs.Bots.Telegram.Core.CastedUpdates;
//using SKitLs.Bots.Telegram.Core.UpdateHandlers;
//using SKitLs.TGBots.Model.Bot.UpdateHandlers.MessgeUpdates;
//using Telegram.Bot.Types.Enums;

//namespace SKitLs.TGBots.Model.Bot.UpdateHandlers
//{
//    internal sealed class DefaultSignedMessageUpdateHandler : ISignedMessageUpdateHandler
//    {
//        /// <summary>
//        /// См. <see cref="ISignedMessageUpdateHandler.TextMessageUpdateHandler"/>.
//        /// По умолчанию <see cref="DefaultTextMessageUpdateHandler"/>
//        /// </summary>
//        public ITextMessageUpdateHandler TextMessageUpdateHandler { get; set; }

//        /// <summary>
//        /// Конструктор по умолчанию
//        /// </summary>
//        public DefaultSignedMessageUpdateHandler()
//        {
//            TextMessageUpdateHandler = new DefaultTextMessageUpdateHandler();
//        }

//        public async Task HandleUpdateAsync(SignedMessageUpdate update)
//        {
//            if (update.Message.Type == MessageType.Text && TextMessageUpdateHandler != null)
//            {
//                await TextMessageUpdateHandler.HandleUpdateAsync(new SignedMessageTextUpdate(update));
//            }
//            // Photo Video Voice etc
//        }
//    }
//}
