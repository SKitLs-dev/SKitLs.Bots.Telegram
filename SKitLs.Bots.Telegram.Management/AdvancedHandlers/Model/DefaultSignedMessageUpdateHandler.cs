using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.MessageUpdates;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Management.AdvancedHandlers.Prototype;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Management.AdvancedHandlers.Model
{
    public class DefaultSignedMessageUpdateHandler : ISignedMessageUpdateHandler
    {
        /// <summary>
        /// См. <see cref="ISignedMessageUpdateHandler.TextMessageUpdateHandler"/>.
        /// По умолчанию <see cref="DefaultTextMessageUpdateHandler"/>
        /// </summary>
        public ITextMessageUpdateHandler TextMessageUpdateHandler { get; set; }
        // PhotoMessage
        // MediaMessage
        // etc

        public DefaultSignedMessageUpdateHandler()
        {
            TextMessageUpdateHandler = new DefaultTextMessageUpdateHandler();
        }

        public async Task HandleUpdateAsync(CastedChatUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();
            
            await HandleUpdateAsync(new SignedMessageUpdate(update.Owner, update, sender));
        }
        public async Task HandleUpdateAsync(SignedMessageUpdate update)
        {
            if (update.Message.Type == MessageType.Text && TextMessageUpdateHandler != null)
            {
                await TextMessageUpdateHandler.HandleUpdateAsync(new SignedMessageTextUpdate(update.Owner, update));
            }
            // Photo Video Voice etc
        }
    }
}