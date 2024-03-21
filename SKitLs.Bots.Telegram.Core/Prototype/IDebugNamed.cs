namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// An interface that offers methods to streamline the debugging process by providing a debug-friendly name.
    /// </summary>
    public interface IDebugNamed
    {
        /// <summary>
        /// Gets a name used to facilitate the debugging process.
        /// </summary>
        public string? DebugName { get; }
    }
}