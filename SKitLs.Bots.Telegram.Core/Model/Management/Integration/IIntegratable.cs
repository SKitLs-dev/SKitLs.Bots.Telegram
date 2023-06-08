using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management.Integration
{
    /// <summary>
    /// TODO. An interface that provides mechanics for integrating external code
    /// into internal <see cref="BotManager"/> structure via <see cref="IBotAction"/>.
    /// </summary>
    /// <typeparam name="TUpdate">Scecific casted update that this class should work with.</typeparam>
    public interface IIntegratable<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// TODO. Collects actions and packs them into a sharable form.
        /// </summary>
        /// <returns>Built collection of provided actions.</returns>
        public IList<IBotAction<TUpdate>> GetActionsList();
    }
}