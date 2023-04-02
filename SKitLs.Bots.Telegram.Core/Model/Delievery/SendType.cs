namespace SKitLs.Bots.Telegram.Core.Model.Delievery
{
    /// <summary>
    /// Type of sending message.
    /// </summary>
    [Obsolete("Post-Check")]
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
