using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Defaults
{
    public class DefaultActionManager<TUpdate> : IActionManager<IBotAction<TUpdate>, TUpdate> where TUpdate : CastedUpdate
    {
        public BotManager Owner { get; set; }
        public List<IBotAction<TUpdate>> Actions { get; private set; }

        public DefaultActionManager(BotManager owner)
        {
            Owner = owner;
            Actions = new();
        }

        public async Task ManageUpdateAsync(TUpdate update)
        {
            foreach (IBotAction<TUpdate> callback in Actions)
                if (callback.ShouldBeExecutedOn(update))
                    await callback.Action(callback, update);
        }
    }
}