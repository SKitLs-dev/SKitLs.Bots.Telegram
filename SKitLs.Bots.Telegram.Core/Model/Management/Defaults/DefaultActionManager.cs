using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Defaults
{
    /// <summary>
    /// Default realization of <see cref="ILinearActionManager{TUpdate}"/>. Provides simple architecture
    /// with linear iteration searcher and one-of-many
    /// <see cref="IBotAction{TUpdate}.ShouldBeExecutedOn(TUpdate)"/> selector.
    /// </summary>
    /// <typeparam name="TUpdate">Scecific casted update that this manager should work with.</typeparam>
    public class DefaultActionManager<TUpdate> : ILinearActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        public string? DebugName { get; set; }

        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public IList<IBotAction<TUpdate>> Actions { get; private set; } = new List<IBotAction<TUpdate>>();
        public List<IBotAction> GetActionsContent() => Actions.Cast<IBotAction>().ToList();

        /// <summary>
        /// Safely adds new action to internal storage.
        /// Verifies it is unique via <see cref="IBotAction.ActionId"/>.
        /// </summary>
        /// <param name="action">Action to be stored</param>
        public void AddSafely(IBotAction<TUpdate> action) => Actions.Add(
            Actions.Contains(action)
            ? throw new DuplicationException(GetType(), typeof(IBotAction<TUpdate>), action.ActionId)
            : action);
        /// <summary>
        /// Safely adds range of actions to internal storage.
        /// Verifies they are unique via <see cref="IBotAction.ActionId"/>.
        /// </summary>
        /// <param name="actions">Actions to be stored</param>
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(act => AddSafely(act));
        public void Apply(IIntegratable<TUpdate> integration) => AddRangeSafely(integration.GetActionsList());

        public async Task ManageUpdateAsync(TUpdate update)
        {
            foreach (IBotAction<TUpdate> callback in Actions)
                if (callback.ShouldBeExecutedOn(update))
                {
                    await callback.Action(update);
                    break;
                }
        }

        public override string? ToString() => DebugName ?? base.ToString();
    }
}