namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    /// <summary>
    /// Represents an update triggered by a message.
    /// </summary>
    public interface IMessageTriggered
    {
        /// <summary>
        /// Gets the ID of the message that triggered the current update.
        /// </summary>
        public int TriggerMessageId { get; }
    }
}