using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    public interface IStatefulActionManager<TUpdate> : IOwnerCompilable, IActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        public IList<IUserState> DeterminedStates => ActionSections
            .SelectMany(x => x.EnabledStates ?? new List<IUserState>())
            .Distinct()
            .ToList();

        public IStateSection<TUpdate> DefaultStateSection { get; }
        public ICollection<IStateSection<TUpdate>> ActionSections { get; }
        public void AddSectionSafely(IStateSection<TUpdate> section);
        public void AddSectionsRangeSafely(ICollection<IStateSection<TUpdate>> sections);

        public void Apply(IIntegratable<TUpdate> integrations) => DefaultStateSection.Apply(integrations);
        public void Apply(IStatefulIntegratable<TUpdate> integrations);
    }
}