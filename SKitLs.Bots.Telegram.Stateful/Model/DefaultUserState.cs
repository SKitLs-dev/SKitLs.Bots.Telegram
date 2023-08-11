using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    /// <summary>
    /// The default implementation of the <see cref="IUserState"/> interface. 
    /// Provides mechanisms for managing user states.
    /// </summary>
    public struct DefaultUserState : IUserState, IEquatable<DefaultUserState>, IComparable<DefaultUserState>
    {
        /// <inheritdoc/>
        public int StateId { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUserState"/> class with the specified data.
        /// </summary>
        /// <param name="id">The identifier of the state.</param>
        /// <param name="name">The name of the state.</param>
        public DefaultUserState(int id, string? name = null)
        {
            StateId = id;
            Name = name ?? "Dynamic no named";
        }

        /// <inheritdoc/>
        public override readonly int GetHashCode() => StateId.GetHashCode() + Name.GetHashCode();

        /// <inheritdoc/>
        public readonly bool Equals(int other) => StateId == other;

        /// <inheritdoc/>
        public readonly bool Equals(DefaultUserState other) => StateId == other.StateId;

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj)
        {
            if (obj is DefaultUserState otherState)
                return Equals(otherState);
            else if (obj is int otherInt)
                return Equals(otherInt);

            return base.Equals(obj);
        }

        /// <inheritdoc/>
        public readonly int CompareTo(int other) => StateId - other;

        /// <inheritdoc/>
        public readonly int CompareTo(DefaultUserState other) => StateId - other.StateId;

        /// <summary>
        /// Determines whether two instances of <see cref="DefaultUserState"/> are equal.
        /// </summary>
        /// <param name="left">The first instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <param name="right">The second instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(DefaultUserState left, DefaultUserState right) => left.Equals(right);
        /// <summary>
        /// Determines whether two instances of <see cref="DefaultUserState"/> are not equal.
        /// </summary>
        /// <param name="left">The first instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <param name="right">The second instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(DefaultUserState left, DefaultUserState right) => !(left == right);
        /// <summary>
        /// Determines whether one instance of <see cref="DefaultUserState"/> is less than the other instance.
        /// </summary>
        /// <param name="left">The first instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <param name="right">The second instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <returns><see langword="true"/> if the first instance is less than the second instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator <(DefaultUserState left, DefaultUserState right) => left.CompareTo(right) < 0;
        /// <summary>
        /// Determines whether one instance of <see cref="DefaultUserState"/> is less than or equal to the other instance.
        /// </summary>
        /// <param name="left">The first instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <param name="right">The second instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <returns><see langword="true"/> if the first instance is less than or equal to the second instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(DefaultUserState left, DefaultUserState right) => left.CompareTo(right) <= 0;
        /// <summary>
        /// Determines whether one instance of <see cref="DefaultUserState"/> is more than the other instance.
        /// </summary>
        /// <param name="left">The first instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <param name="right">The second instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <returns><see langword="true"/> if the first instance is more than the second instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator >(DefaultUserState left, DefaultUserState right) => left.CompareTo(right) > 0;
        /// <summary>
        /// Determines whether one instance of <see cref="DefaultUserState"/> is more than or equal to the other instance.
        /// </summary>
        /// <param name="left">The first instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <param name="right">The second instance of <see cref="DefaultUserState"/> to compare.</param>
        /// <returns><see langword="true"/> if the first instance is more than or equal to the second instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(DefaultUserState left, DefaultUserState right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Implicit operator <see cref="DefaultUserState"/> => <see cref="int"/>
        /// </summary>
        /// <param name="id">User state's id.</param>
        public static implicit operator DefaultUserState(int id) => new(id, "generic implicit");
        /// <summary>
        /// Implicit operator <see cref="int"/> => <see cref="DefaultUserState"/>. Get state's id.
        /// </summary>
        /// <param name="state">State to get id.</param>
        public static implicit operator int(DefaultUserState state) => state.StateId;

        /// <inheritdoc/>
        public override readonly string ToString() => $"({StateId}) {Name}";
    }
}