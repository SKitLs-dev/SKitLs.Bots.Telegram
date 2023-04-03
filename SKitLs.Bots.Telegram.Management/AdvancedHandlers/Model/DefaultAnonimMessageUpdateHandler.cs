using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Management.AdvancedHandlers.Model
{
    public class DefaultAnonimMessageUpdateHandler : IUpdateHandlerBase<AnonimMessageUpdate>
    {
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