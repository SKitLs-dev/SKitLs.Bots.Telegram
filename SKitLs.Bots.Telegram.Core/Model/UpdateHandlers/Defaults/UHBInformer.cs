using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults
{
    internal class UHBInformer : IUpdateHandlerBase
    {
        public string UpdateName { get; private set; }
        public bool UseLogger { get; private set; }
        public bool InformInChat { get; private set; }

        public UHBInformer(string updateName, bool log = false, bool inform = false)
        {
            UpdateName = updateName;
            UseLogger = log;
            InformInChat = inform;
        }

        public async Task HandleUpdateAsync(CastedChatUpdate update, IBotUser? sender)
        {
            string mes = $"Handled update: {UpdateName}";
            if (UseLogger) { update.Logger.Log(mes); }
            if (InformInChat) await update.SendMessageTriggerToChatAsync(mes);
        }
    }
}
