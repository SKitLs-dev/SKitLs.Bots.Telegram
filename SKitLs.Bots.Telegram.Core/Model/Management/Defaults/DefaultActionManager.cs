using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Defaults
{
    public class DefaultActionManager<TUpdate> : IActionManager<IBotAction<TUpdate>, TUpdate> where TUpdate : CastedUpdate
    {
        public BotManager Owner { get; set; } = null!;
        public List<IBotAction<TUpdate>> Actions { get; private set; }

        public DefaultActionManager()
        {
            Actions = new();
        }
        public void Compile(BotManager manager)
        {
            Owner = manager;
        }

        public async Task ManageUpdateAsync(TUpdate update)
        {
            foreach (IBotAction<TUpdate> callback in Actions)
                if (callback.ShouldBeExecutedOn(update))
                    await callback.Action(callback, update);
        }
    }
}