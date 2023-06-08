namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IDebugNamed
    {
        /// <summary>
        /// Used for simplified dubugging proccess.
        /// </summary>
        public string? DebugName { get; }
    }
}