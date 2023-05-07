using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management
{
    public interface IActionManager<TAction, TUpdate> where TAction : IBotAction<TUpdate> where TUpdate : CastedUpdate
    {
        public BotManager Owner { get; }
        public void Compile(BotManager manager);

        public List<TAction> Actions { get; }
        public Task ManageUpdateAsync(TUpdate update);
    }
}