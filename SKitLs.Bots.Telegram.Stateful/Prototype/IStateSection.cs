using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// Provides mechanics of declaring specific state sections, which are used to fragment handling logics into parts, based on
    /// user's states.
    /// </summary>
    /// <typeparam name="TUpdate">Type of update that this section works with.</typeparam>
    public interface IStateSection<TUpdate> : IDebugNamed, IActionsHolder, IIntegratable<TUpdate>, IEquatable<IStateSection<TUpdate>>, IEnumerable<IBotAction<TUpdate>> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Determines states that collected actions should react on.
        /// </summary>
        public IList<IUserState>? EnabledStates { get; }
        /// <summary>
        /// Determines, whether collected action should be executed regardless user state.
        /// </summary>
        public bool EnabledAny { get; }
        /// <summary>
        /// Enables custom state.
        /// </summary>
        /// <param name="state">State to enable.</param>
        public void EnableState(IUserState state);
        /// <summary>
        /// Determines, whether this section should be executed with <paramref name="state"/>.
        /// </summary>
        /// <param name="state">State to analyze.</param>
        /// <returns><see langword="true"/> if section is enabled with <paramref name="state"/>. Otherwise <see langword="false"/>.</returns>
        public bool IsEnabledWith(IUserState state) => EnabledAny || EnabledStates!.ToList().Contains(state);

        /// <summary>
        /// Safely adds <paramref name="action"/> to internal storage.
        /// </summary>
        /// <param name="action">Action to be added.</param>
        public void AddSafely(IBotAction<TUpdate> action);
        /// <summary>
        /// Safely adds range of <paramref name="actions"/> to internal storage.
        /// </summary>
        /// <param name="actions">Actions to be added.</param>
        public void AddRangeSafely(IList<IBotAction<TUpdate>> actions);
        public void Apply(IIntegratable<TUpdate> integration);
    }
}