using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Management.Managers
{
    public interface IActionManager<TAction, TUpdate> where TAction : IBotInteraction where TUpdate : CastedUpdate
    {
        public List<TAction> Actions { get; }
        public Task HandleUpdateAsync(TUpdate update);
    }
}