using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    public interface IUpdateHandlerBase
    {
        public BotManager Owner { get; }
        public Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender);
    }
    public interface IUpdateHandlerBase<TUpdate> : IUpdateHandlerBase where TUpdate : ICastedUpdate
    {
        public TUpdate BuildUpdate(CastedUpdate update, IBotUser? sender);
        public Task HandleUpdateAsync(TUpdate update);
    }
}