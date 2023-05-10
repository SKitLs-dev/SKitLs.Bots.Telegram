using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    public class UHBInformer<TUpdate> : IUpdateHandlerBase<TUpdate> where TUpdate : class, ICastedUpdate
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public string UpdateName { get; private set; }
        public bool UseLogger { get; set; }
        public bool InformInChat { get; set; }

        public UHBInformer(string updateName, bool log = false, bool inform = false)
        {
            UpdateName = updateName;
            UseLogger = log;
            InformInChat = inform;
        }

        public async Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
            => await HandleUpdateAsync(BuildUpdate(update, sender));

        public TUpdate BuildUpdate(CastedUpdate update, IBotUser? sender) => (update as TUpdate);

        public async Task HandleUpdateAsync(TUpdate update)
        {
            string mes = $"Handled update: {UpdateName}";
            if (UseLogger) { Owner.LocalLogger.Log(mes); }
            //if (InformInChat) await Owner.DelieveryService.SendMessageTriggerToChatAsync(mes);
        }
    }
}
