using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// Represents specific stateful bot user. Can be extended with additional functional.
    /// </summary>
    public interface IStatefulUser : IBotUser
    {
        /// <summary>
        /// Represents current user state.
        /// </summary>
        public IUserState State { get; set; }
        /// <summary>
        /// Resets user state to default one.
        /// </summary>
        public void ResetState();
    }
}