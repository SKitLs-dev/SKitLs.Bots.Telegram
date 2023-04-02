using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Management.Managers.Model
{
    internal class DefaultCallbackManager : IActionManager<IBotCallback, SignedCallbackUpdate>
    {
        public List<IBotCallback> Actions { get; set; }

        public DefaultCallbackManager()
        {
            Actions = new();
        }

        public async Task HandleUpdateAsync(SignedCallbackUpdate update)
        {
            foreach (IBotCallback callback in Actions)
                if (callback.ShouldBeExecutedOn(update.Data))
                        await callback.Action(callback, update);
        }
    }
}