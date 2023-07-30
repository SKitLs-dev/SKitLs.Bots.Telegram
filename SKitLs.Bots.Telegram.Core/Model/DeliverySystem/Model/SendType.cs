namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model
{
    /// <summary>
    /// Type of sending message.
    /// </summary>
    [Obsolete("Will be implemented and redesigned in future versions.")]
    public enum SendType
    {
        /// <summary>
        /// Declares Text-Message type
        /// </summary>
        Text,

        /// <summary>
        /// Declares Text-Message Editing type
        /// </summary>
        Edit,

        /// <summary>
        /// Declares Photo-Message type
        /// </summary>
        Photo
    }
}
