namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    /// <summary>
    /// An interface that provides methods to simplify dubugging proccess.
    /// </summary>
    public interface IDebugNamed
    {
        /// <summary>
        /// Used for simplified dubugging proccess.
        /// </summary>
        public string? DebugName { get; }
    }
}