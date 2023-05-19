namespace SKitLs.Bots.Telegram.Stateful.Prototype
{
    public interface IUserState : IEquatable<int>, IComparable<int>
    {
        public int StateId { get; }
        public string Name { get; }
    }
}