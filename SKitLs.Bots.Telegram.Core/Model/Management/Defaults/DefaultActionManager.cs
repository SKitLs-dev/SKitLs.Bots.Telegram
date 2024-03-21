using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Defaults
{
    // XML-Doc Update
    /// <summary>
    /// Default realization of <see cref="ILinearActionManager{TUpdate}"/>. Provides simple architecture
    /// with linear iteration searcher and one-of-many
    /// <see cref="IBotAction{TUpdate}.ShouldBeExecutedOn(TUpdate)"/> selector.
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this manager should work with.</typeparam>
    public class DefaultActionManager<TUpdate> : ILinearActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Name, used for simplifying debugging process.
        /// </summary>
        public string? DebugName { get; set; }

        /// <summary>
        /// Determines whether <see cref="DefaultActionManager{TUpdate}"/> should break iterator after first matched action was executed.
        /// </summary>
        public bool OnlyOneAction { get; set; }

        private BotManager? _owner;
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <summary>
        /// Specified method that raised during reflective <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/> compilation.
        /// Declare it to extend preset functionality.
        /// Invoked after <see cref="Owner"/> updating, but before recursive update.
        /// </summary>
        public Action<object, BotManager>? OnCompilation => null;
        
        /// <summary>
        /// An internal storage used to store saved actions.
        /// </summary>
        public IList<IBotAction<TUpdate>> Actions { get; } = new List<IBotAction<TUpdate>>();

        /// <summary>
        /// Collects all <see cref="IBotAction"/>s declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public List<IBotAction> GetHeldActions() => Actions.Cast<IBotAction>().ToList();

        /// <summary>
        /// Safely adds new action to internal storage.
        /// Verifies it is unique via <see cref="IBotAction.ActionId"/>.
        /// </summary>
        /// <param name="action">Action to be stored.</param>
        public void AddSafely(IBotAction<TUpdate> action) => Actions.Add(
            Actions.Contains(action)
            ? throw new DuplicationException(GetType(), typeof(IBotAction<TUpdate>), action.ActionId)
            : action);
        /// <summary>
        /// Safely adds range of actions to internal storage.
        /// Verifies they are unique via <see cref="IBotAction.ActionId"/>.
        /// </summary>
        /// <param name="actions">Actions to be stored.</param>
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions) => actions
            .ToList()
            .ForEach(act => AddSafely(act));

        /// <summary>
        /// Manages incoming update, delegating it to one of a stored actions.
        /// </summary>
        /// <param name="update">Update to be handled.</param>
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

        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override string? ToString() => DebugName ?? base.ToString();
    }
}