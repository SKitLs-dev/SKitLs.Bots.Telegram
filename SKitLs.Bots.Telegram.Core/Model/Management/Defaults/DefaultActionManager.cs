using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Defaults
{
    public class DefaultActionManager<TUpdate> : IActionManager<IBotAction<TUpdate>, TUpdate> where TUpdate : CastedUpdate
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public List<IBotAction<TUpdate>> Actions { get; private set; } = new();

        public async Task ManageUpdateAsync(TUpdate update)
        {
            foreach (IBotAction<TUpdate> callback in Actions)
                if (callback.ShouldBeExecutedOn(update))
                    await callback.Action(callback, update);
        }

        public Action<object, BotManager>? OnCompilation => OnCompilationAction;
        public void OnCompilationAction(object sender, BotManager owner)
        {
            preCompileActions.ForEach(x =>
            {
                x.Owner = owner;
                x.GetActionsList().ForEach(act => Actions.Add(act));
            });
        }

        private readonly List<ITgActorList<TUpdate>> preCompileActions = new();
        public void Apply(ITgActorList<TUpdate> actions) => preCompileActions.Add(actions);
    }
}