using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.AdvancedHandlers.Defaults
{
    public class DefaultAnonimMessageUpdateHandler : IUpdateHandlerBase<AnonimMessageUpdate>
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public IUpdateHandlerBase<AnonimMessageTextUpdate>? TextMessageUpdateHandler { get; set; }

        public DefaultAnonimMessageUpdateHandler()
        {
            TextMessageUpdateHandler = new DefaultAnonimMessageTextUpdateHandler();
        }

        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(BuildUpdate(update, sender));
        public AnonimMessageUpdate BuildUpdate(CastedUpdate update, IBotUser? sender) => new(update);

        public async Task HandleUpdateAsync(AnonimMessageUpdate update)
        {
            if (update.Message.Type == MessageType.Text && TextMessageUpdateHandler != null)
            {
                await TextMessageUpdateHandler.HandleUpdateAsync(new AnonimMessageTextUpdate(update));
            }
        }
    }
}