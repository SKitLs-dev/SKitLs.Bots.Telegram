using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    public class UHBInformer : IUpdateHandlerBase<CastedUpdate>
    {
        public BotManager Owner { get; private set; }

        public string UpdateName { get; private set; }
        public bool UseLogger { get; set; }
        public bool InformInChat { get; set; }

        public UHBInformer(BotManager owner, string updateName, bool log = false, bool inform = false)
        {
            Owner = owner;
            UpdateName = updateName;
            UseLogger = log;
            InformInChat = inform;
        }

        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(BuildUpdate(update, sender));

        public CastedUpdate BuildUpdate(CastedUpdate update, IBotUser? sender) => update;

        public async Task HandleUpdateAsync(CastedUpdate update)
        {
            string mes = $"Handled update: {UpdateName}";
            if (UseLogger) { Owner.LocalLogger.Log(mes); }
            //if (InformInChat) await Owner.DelieveryService.SendMessageTriggerToChatAsync(mes);
        }
    }
}
