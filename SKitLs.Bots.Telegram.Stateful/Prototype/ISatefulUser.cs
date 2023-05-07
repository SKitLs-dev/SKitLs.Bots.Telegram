using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    public interface ISatefulUser : IBotUser
    {
        public int StateID { get; }
    }
}
