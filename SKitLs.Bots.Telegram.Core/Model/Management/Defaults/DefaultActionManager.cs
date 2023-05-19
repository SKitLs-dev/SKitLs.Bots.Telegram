using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using System.Collections.ObjectModel;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Defaults
{
    public class DefaultActionManager<TUpdate> : ILinearActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        public string? DebugName { get; set; }

        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public ICollection<IBotAction<TUpdate>> Actions { get; private set; } = new Collection<IBotAction<TUpdate>>();

        public async Task ManageUpdateAsync(TUpdate update)
        {
            foreach (IBotAction<TUpdate> callback in Actions)
                if (callback.ShouldBeExecutedOn(update))
                    await callback.Action(callback, update);
        }

        // TODO
        public void AddSafely(IBotAction<TUpdate> action) => Actions.Add(
            Actions.Contains(action)
            ? throw new Exception()
            : action);
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(act => AddSafely(act));
        public void Apply(IIntegratable<TUpdate> integration) => AddRangeSafely(integration.GetActionsList());

        public override string? ToString() => DebugName ?? base.ToString();
    }
}