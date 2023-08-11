using SKitLs.Bots.Telegram.Stateful.Model;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// Defines the mechanism for user states. The default implementation is <see cref="DefaultUserState"/>.
    /// </summary>
    public interface IUserState : IEquatable<int>, IComparable<int>
    {
        /// <summary>
        /// Gets the unique identifier of the state.
        /// </summary>
        public int StateId { get; }

        /// <summary>
        /// Gets the display name of the state.
        /// </summary>
        public string Name { get; }
    }
}