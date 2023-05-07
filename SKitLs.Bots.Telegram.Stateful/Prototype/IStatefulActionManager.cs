using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    public interface IStatefulActionManager<TAction, TUpdate> : IActionManager<TAction, TUpdate> where TAction : IBotAction<TUpdate> where TUpdate : CastedUpdate
    {
    }
}