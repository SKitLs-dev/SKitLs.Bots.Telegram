using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// An interface that provides complex logic for handling updates via <see cref="IStateSection{TUpdate}"/>
    /// and <see cref="IBotAction"/> interactions. Stores interactions and delegates incoming updates to stored actions.
    /// Provides simple architecture with one storage collection.
    /// <para>
    /// Fourth architecture level, addon for <see cref="IActionManager{TUpdate}"/>
    /// </para>
    /// <para>Inherits: <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/></para>
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this manager should work with.</typeparam>
    public interface IStatefulActionManager<TUpdate> : IOwnerCompilable, IActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Represents all the states, defined in the manager.
        /// </summary>
        public IList<IUserState> DeterminedStates => ActionSections
            .SelectMany(x => x.EnabledStates ?? new List<IUserState>())
            .Distinct()
            .ToList();

        /// <summary>
        /// State section that is defined as a default one.
        /// </summary>
        public IStateSection<TUpdate> DefaultStateSection { get; }
        /// <summary>
        /// Internal collection used for storing action sections. 
        /// </summary>
        public ICollection<IStateSection<TUpdate>> ActionSections { get; }
        /// <summary>
        /// Safely adds new state section.
        /// </summary>
        /// <param name="section">Section to add.</param>
        public void AddSectionSafely(IStateSection<TUpdate> section);
        /// <summary>
        /// Safely adds a range of state section.
        /// </summary>
        /// <param name="sections">Sections to add.</param>
        public void AddSectionsRangeSafely(ICollection<IStateSection<TUpdate>> sections);

        /// <summary>
        /// Applies and integrates custom class that supports <see cref="IIntegratable{TUpdate}"/>.
        /// </summary>
        /// <param name="integration">An item to be integrated.</param>
        [Obsolete("Will be removed in future versions. Use IApplicant instead.", true)]
        public void Apply(IStatefulIntegratable<TUpdate> integration);
    }
}