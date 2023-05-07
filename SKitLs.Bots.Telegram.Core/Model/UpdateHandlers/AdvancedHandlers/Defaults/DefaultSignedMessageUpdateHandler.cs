using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.AdvancedHandlers.Defaults
{
    public class DefaultSignedMessageUpdateHandler : IUpdateHandlerBase<SignedMessageUpdate>
    {
        public BotManager Owner { get; private set; } = null!;

        public IUpdateHandlerBase<SignedMessageTextUpdate>? TextMessageUpdateHandler { get; set; }

        // PhotoMessage
        // MediaMessage
        // etc

        public DefaultSignedMessageUpdateHandler()
        {
            TextMessageUpdateHandler = new DefaultSignedMessageTextUpdateHandler();
        }

        public void Compile(BotManager manager)
        {
            Owner = manager;
            TextMessageUpdateHandler?.Compile(manager);
        }

        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(BuildUpdate(update, sender));

        public SignedMessageUpdate BuildUpdate(CastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();

            return new(update, sender);
        }
        public async Task HandleUpdateAsync(SignedMessageUpdate update)
        {
            if (update.Message.Type == MessageType.Text && TextMessageUpdateHandler != null)
            {
                await TextMessageUpdateHandler.HandleUpdateAsync(new SignedMessageTextUpdate(update));
            }
            // Photo Video Voice etc
        }
    }
}