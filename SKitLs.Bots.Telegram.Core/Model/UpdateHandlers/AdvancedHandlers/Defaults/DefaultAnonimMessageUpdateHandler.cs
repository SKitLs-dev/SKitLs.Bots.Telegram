using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Management.AdvancedHandlers.Defaults;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.AdvancedHandlers.Defaults
{
    public class DefaultAnonimMessageUpdateHandler : IUpdateHandlerBase<AnonimMessageUpdate>
    {
        public BotManager Owner { get; private set; }

        public IUpdateHandlerBase<AnonimMessageTextUpdate>? TextMessageUpdateHandler { get; set; }

        public DefaultAnonimMessageUpdateHandler(BotManager owner)
        {
            Owner = owner;
            TextMessageUpdateHandler = new DefaultAnonimMessageTextUpdateHandler(owner);
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