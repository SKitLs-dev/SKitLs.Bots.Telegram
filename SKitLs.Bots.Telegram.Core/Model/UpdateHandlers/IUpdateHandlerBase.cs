using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    public interface IUpdateHandlerBase
    {
        public Task HandleUpdateAsync(CastedChatUpdate update, IBotUser? sender);
    }
}
