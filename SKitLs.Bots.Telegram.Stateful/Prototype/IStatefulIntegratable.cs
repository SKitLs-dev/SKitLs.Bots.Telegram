using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    [Obsolete("Use \"ApplyableFor\" instead")]
    public interface IStatefulIntegratable<TUpdate> where TUpdate : ICastedUpdate
    {
        public ICollection<IStateSection<TUpdate>> GetSectionsList();
    }
}