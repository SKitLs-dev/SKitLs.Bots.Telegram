using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Management.AdvancedHandlers.Prototype
{
    public interface IAnonMessageUpdateHandler : IUpdateHandlerBase
    {
        public ITextMessageUpdateHandler TextMessageUpdateHandler { get; }
        public void UseCustomTextMessageUpdateHandler(ITextMessageUpdateHandler handler);
        public Task HandleUpdateAsync(MessageUpdate update);
    }
}