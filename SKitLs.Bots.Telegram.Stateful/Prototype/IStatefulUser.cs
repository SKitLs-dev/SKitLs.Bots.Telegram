using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// Represents a specific stateful bot user. This interface can be extended with additional functionality.
    /// </summary>
    public interface IStatefulUser : IBotUser
    {
        /// <summary>
        /// Represents the current user state.
        /// </summary>
        public IUserState State { get; set; }

        /// <summary>
        /// Resets the user state to the default one.
        /// </summary>
        public void ResetState();
    }
}