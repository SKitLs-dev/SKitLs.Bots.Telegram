using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// An interface providing complex logic for handling updates via interactions between <see cref="IStateSection{TUpdate}"/>
    /// and <see cref="IBotAction"/> instances. This interface manages interactions and delegates incoming updates to stored actions,
    /// offering a straightforward architecture with a single storage collection.
    /// <para>
    /// Fourth level of architecture, serves as an addon for <see cref="IActionManager{TUpdate}"/>.
    /// </para>
    /// <para>Inherits: <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/></para>
    /// </summary>
    /// <typeparam name="TUpdate">The specific casted update that this manager should work with.</typeparam>
    public interface IStatefulActionManager<TUpdate> : IOwnerCompilable, IActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// The state section that is defined as the default one.
        /// </summary>
        public IStateSection<TUpdate> DefaultStateSection { get; }
        
        /// <summary>
        /// An internal collection used for storing action sections. 
        /// </summary>
        public IEnumerable<IStateSection<TUpdate>> GetActionSections();

        /// <summary>
        /// Represents all the states defined in the manager.
        /// </summary>
        public IEnumerable<IUserState> GetDeterminedStates() => GetActionSections()
            .SelectMany(x => x.GetEnabledStates())
            .Distinct()
            .ToList();

        /// <summary>
        /// Safely adds a new state section.
        /// </summary>
        /// <param name="section">The section to add.</param>
        public void AddSectionSafely(IStateSection<TUpdate> section);

        /// <summary>
        /// Safely adds a range of state sections.
        /// </summary>
        /// <param name="sections">The sections to add.</param>
        public void AddSectionsRangeSafely(ICollection<IStateSection<TUpdate>> sections);
    }
}