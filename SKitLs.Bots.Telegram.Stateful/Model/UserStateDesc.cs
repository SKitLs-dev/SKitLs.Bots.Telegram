namespace SKitLs.Bots.Telegram.Stateful.Model
{
    public class UserStateDesc
    {
        public int StateId { get; set; }
        public string Name { get; set; }

        public UserStateDesc(int id, string name = "No name")
        {
            StateId = id;
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            if (obj != null)
            {
                if (obj is UserStateDesc us)
                    return us.StateId == StateId;
            }
            return base.Equals(obj);
        }
        public override string ToString() => $"{StateId} {Name}";

        public static implicit operator UserStateDesc(int id) => new(id);
        public static implicit operator int(UserStateDesc state) => state.StateId;
    }
}
