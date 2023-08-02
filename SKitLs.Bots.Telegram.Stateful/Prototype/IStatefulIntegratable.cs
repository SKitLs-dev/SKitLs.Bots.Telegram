using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// TODO. An interface that provides mechanics for integrating external code
    /// into internal <see cref="BotManager"/> structure via <see cref="IBotAction"/>.
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this class should work with.</typeparam>
    [Obsolete("Will be removed in future versions. Use IApplicant instead.", true)]
    public interface IStatefulIntegratable<TUpdate> where TUpdate : ICastedUpdate
    {
        public ICollection<IStateSection<TUpdate>> GetSectionsList();
    }
}