using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Defaults
{
    /// <summary>
    /// Default implementation of <see cref="ILinearActionManager{TUpdate}"/>. Provides a simple architecture
    /// with linear iteration search and a one-of-many selector for <see cref="IBotAction{TUpdate}.ShouldBeExecutedOn(TUpdate)"/>.
    /// </summary>
    /// <typeparam name="TUpdate">The specific casted update type that this manager works with.</typeparam>
    public class LinearActionManager<TUpdate> : OwnedObject, ILinearActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <inheritdoc/>
        public string? DebugName { get; set; }

        /// <summary>
        /// Determines whether <see cref="LinearActionManager{TUpdate}"/> should break the iterator after the first matched action is executed.
        /// </summary>
        public bool OnlyOneAction { get; set; }

        /// <inheritdoc/>
        public IList<IBotAction<TUpdate>> Actions { get; } = new List<IBotAction<TUpdate>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearActionManager{TUpdate}"/> class with optional debug name and setting for executing only one action.
        /// </summary>
        /// <param name="debugName">Optional name used for debugging purposes.</param>
        /// <param name="onlyOneAction">Optional flag indicating whether only one action should be executed.</param>
        public LinearActionManager(string? debugName = null, bool onlyOneAction = true)
        {
            DebugName = debugName;
            OnlyOneAction = onlyOneAction;
        }

        /// <inheritdoc/>
        public List<IBotAction> GetHeldActions() => Actions.Cast<IBotAction>().ToList();

        /// <inheritdoc/>
        public void AddSafely(IBotAction<TUpdate> action) => Actions.Add(
            Actions.Contains(action)
            ? throw new DuplicationException(GetType(), typeof(IBotAction<TUpdate>), action.ActionId)
            : action);

        /// <inheritdoc/>
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(act => AddSafely(act));

        /// <inheritdoc/>
        public async Task ManageUpdateAsync(TUpdate update)
        {
            foreach (IBotAction<TUpdate> callback in Actions)
                if (callback.ShouldBeExecutedOn(update))
                {
                    await callback.Action(update);
                    if (OnlyOneAction)
                        break;
                }
        }

        /// <inheritdoc/>
        public override string? ToString() => DebugName ?? base.ToString();
    }
}