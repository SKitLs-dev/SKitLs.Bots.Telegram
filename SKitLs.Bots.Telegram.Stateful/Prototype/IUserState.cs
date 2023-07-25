using SKitLs.Bots.Telegram.Stateful.Model;

namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    /// <summary>
    /// Provides mechanics of user states. Default: <see cref="DefaultUserState"/>.
    /// </summary>
    public interface IUserState : IEquatable<int>, IComparable<int>
    {
        /// <summary>
        /// Represents state's unique identifier.
        /// </summary>
        public int StateId { get; }
        /// <summary>
        /// State's display name.
        /// </summary>
        public string Name { get; }
    }
}