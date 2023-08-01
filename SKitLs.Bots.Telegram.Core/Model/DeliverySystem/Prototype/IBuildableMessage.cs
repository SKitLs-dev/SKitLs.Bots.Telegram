namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype
{
    /// <summary>
    /// An interface that provides ways of getting message's text.
    /// </summary>
    public interface IBuildableMessage : ICloneable
    {
        /// <summary>
        /// Builds object's data and packs it into one text so it could be easily sent to server.
        /// </summary>
        /// <returns>Valid text, ready to be sent.</returns>
        [Obsolete("Will be replaced with IDynamic methods from *.AdvancedMessages")]
        public string GetMessageText();
    }
}