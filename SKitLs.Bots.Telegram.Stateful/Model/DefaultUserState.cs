using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    public struct DefaultUserState : IUserState, IEquatable<DefaultUserState>, IComparable<DefaultUserState>
    {
        public int StateId { get; set; }
        public string Name { get; set; }

        public DefaultUserState(int id, string name = "Dynamic no named")
        {
            StateId = id;
            Name = name;
        }

        public override int GetHashCode() => StateId.GetHashCode() + Name.GetHashCode();
        public bool Equals(int other) => StateId == other;
        public bool Equals(DefaultUserState other) => StateId == other;
        public override bool Equals(object? obj)
        {
            if (obj is DefaultUserState otherState)
                return Equals(otherState);
            else if (obj is int otherInt)
                return Equals(otherInt);

            return base.Equals(obj);
        }
        public int CompareTo(int other) => StateId - other;
        public int CompareTo(DefaultUserState other) => StateId - other;

        public static bool operator ==(DefaultUserState left, DefaultUserState right) => left.Equals(right);
        public static bool operator !=(DefaultUserState left, DefaultUserState right) => !(left == right);
        public static bool operator <(DefaultUserState left, DefaultUserState right) => left.CompareTo(right) < 0;
        public static bool operator <=(DefaultUserState left, DefaultUserState right) => left.CompareTo(right) <= 0;
        public static bool operator >(DefaultUserState left, DefaultUserState right) => left.CompareTo(right) > 0;
        public static bool operator >=(DefaultUserState left, DefaultUserState right) => left.CompareTo(right) >= 0;

        public static implicit operator DefaultUserState(int id) => new(id, "generic implict");
        public static implicit operator int(DefaultUserState state) => state.StateId;
        public override string ToString() => $"{StateId}. {Name}";
    }
}