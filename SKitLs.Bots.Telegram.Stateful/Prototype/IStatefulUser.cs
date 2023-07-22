using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    public interface IStatefulUser : IBotUser
    {
        public IUserState State { get; set; }
        public void ResetState();
    }
}