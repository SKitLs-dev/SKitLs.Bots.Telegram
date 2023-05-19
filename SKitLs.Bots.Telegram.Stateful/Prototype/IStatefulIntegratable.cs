using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    public interface IStatefulIntegratable<TUpdate> where TUpdate : ICastedUpdate
    {
        public ICollection<IStateSection<TUpdate>> GetSectionsList();
    }
}