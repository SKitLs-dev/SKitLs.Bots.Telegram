using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// Provides the mechanics of declaring specific state sections, which are used to fragment handling logics into parts,
    /// based on the user's states.
    /// </summary>
    /// <typeparam name="TUpdate">The type of update that this section works with.</typeparam>
    public interface IStateSection<TUpdate> : IDebugNamed, IBotActionsHolder, IEquatable<IStateSection<TUpdate>>, IEnumerable<IBotAction<TUpdate>> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Determines whether the collected actions should be executed regardless of the user's state.
        /// </summary>
        public bool EnabledAny { get; }

        /// <summary>
        /// Retrieves a list of all user states that this section is permitted to react to.
        /// </summary>
        /// <returns>A list of enabled user states.</returns>
        public List<IUserState> GetEnabledStates();

        /// <summary>
        /// Enables a custom state.
        /// </summary>
        /// <param name="state">The state to enable.</param>
        public void EnableState(IUserState state);

        /// <summary>
        /// Determines whether this section should be executed with the given <paramref name="state"/>.
        /// </summary>
        /// <param name="state">The state to analyze.</param>
        /// <returns><see langword="true"/> if the section is enabled with the specified <paramref name="state"/>; otherwise, <see langword="false"/>.</returns>
        public bool IsEnabledWith(IUserState state) => EnabledAny || GetEnabledStates().Contains(state);

        /// <summary>
        /// Collects all <see cref="IBotAction{TUpdate}"/> declared in the class.
        /// </summary>
        /// <returns>A collected list of declared actions.</returns>
        public List<IBotAction<TUpdate>> GetActionsList();

        /// <summary>
        /// Safely adds the <paramref name="action"/> to the internal storage.
        /// </summary>
        /// <param name="action">The action to be added.</param>
        public void AddSafely(IBotAction<TUpdate> action);

        /// <summary>
        /// Safely adds a range of <paramref name="actions"/> to the internal storage.
        /// </summary>
        /// <param name="actions">The collection of actions to be added.</param>
        public void AddRangeSafely(IEnumerable<IBotAction<TUpdate>> actions);

        /// <summary>
        /// Safely adds data of a new <paramref name="section"/> to the internal storage.
        /// </summary>
        /// <param name="section">The section to be merged.</param>
        public void MergeSafely(IStateSection<TUpdate> section);
    }
}