using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management.Integration;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Management
{
    public interface ILinearActionManager<TUpdate> : IOwnerCompilable, IActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        public ICollection<IBotAction<TUpdate>> Actions { get; }
        public void Apply(IIntegratable<TUpdate> actions);
    }
}