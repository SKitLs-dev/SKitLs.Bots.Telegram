using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    /// <summary>
    /// Default realization of <see cref="IUserState"/>. Provides mechanics of user states.
    /// </summary>
    public struct DefaultUserState : IUserState, IEquatable<DefaultUserState>, IComparable<DefaultUserState>
    {
        /// <summary>
        /// Represents state's unique identifier.
        /// </summary>
        public int StateId { get; set; }
        /// <summary>
        /// State's display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DefaultUserState"/> with specified data.
        /// </summary>
        /// <param name="id">State's id.</param>
        /// <param name="name">State's name.</param>
        public DefaultUserState(int id, string? name = null)
        {
            StateId = id;
            Name = name ?? "Dynamic no named";
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override readonly int GetHashCode() => StateId.GetHashCode() + Name.GetHashCode();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other"></param>
        /// <returns><see langword="true"/> if the current object is equal to the <paramref name="other"/>;
        /// otherwise <see langword="false"/></returns>
        public readonly bool Equals(int other) => StateId == other;

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other"></param>
        /// <returns><see langword="true"/> if the current object is equal to the <paramref name="other"/>;
        /// otherwise <see langword="false"/></returns>
        public readonly bool Equals(DefaultUserState other) => StateId == other.StateId;

        /// <summary>
        /// Indicates whether this instance and a specified object are equal. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> and this instance are the same type and
        /// represent the same value; otherwise <see langword="false"/></returns>
        public override readonly bool Equals(object? obj)
        {
            if (obj is DefaultUserState otherState)
                return Equals(otherState);
            else if (obj is int otherInt)
                return Equals(otherInt);

            return base.Equals(obj);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns
        /// an integer that indicates whether the current instance precedes, follows, or
        /// occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <para><c>Value</c> – Meaning</para>
        /// <para><c>Less than zero</c> – This instance precedes other in the sort order.</para>
        /// <para><c>Zero</c> – This instance occurs in the same position in the sort order as other.</para>
        /// <para><c>Greater than zero</c> – This instance follows other in the sort order.</para></returns>
        public readonly int CompareTo(int other) => StateId - other;

        /// <summary>
        /// Compares the current instance with another object of the same type and returns
        /// an integer that indicates whether the current instance precedes, follows, or
        /// occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <para><c>Value</c> – Meaning</para>
        /// <para><c>Less than zero</c> – This instance precedes other in the sort order.</para>
        /// <para><c>Zero</c> – This instance occurs in the same position in the sort order as other.</para>
        /// <para><c>Greater than zero</c> – This instance follows other in the sort order.</para></returns>
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

        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override readonly string ToString() => $"({StateId}) {Name}";
    }
}