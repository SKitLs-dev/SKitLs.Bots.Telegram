﻿using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.AdvancedHandlers.Defaults
{
    public class DefaultAnonimMessageUpdateHandler : IUpdateHandlerBase<AnonimMessageUpdate>
    {
        public BotManager Owner { get; private set; } = null!; 

        public IUpdateHandlerBase<AnonimMessageTextUpdate>? TextMessageUpdateHandler { get; set; }

        public DefaultAnonimMessageUpdateHandler()
        {
            TextMessageUpdateHandler = new DefaultAnonimMessageTextUpdateHandler();
        }
        public void Compile(BotManager manager)
        {
            Owner = manager;
            TextMessageUpdateHandler?.Compile(manager);
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