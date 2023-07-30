namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// An interface that provides methods to simplify dubugging proccess.
    /// </summary>
    public interface IDebugNamed
    {
        /// <summary>
        /// Name, used for simplifying dubugging proccess.
        /// </summary>
        public string? DebugName { get; }
    }
}