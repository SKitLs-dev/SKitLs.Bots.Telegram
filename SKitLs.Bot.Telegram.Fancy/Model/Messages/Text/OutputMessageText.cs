using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text
{
    /// <summary>
    /// Default realization of <see cref="IOutputMessage"/>. Derived from abstract <see cref="OutputMessage"/>.
    /// Represents an advanced Text Message that can be processed by <see cref="AdvancedDeliverySystem"/>.
    /// </summary>
    public class OutputMessageText : OutputMessage
    {
        /// <summary>
        /// Represents message's text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="OutputMessageText"/> with specified data.
        /// </summary>
        /// <param name="text">Message's text.</param>
        public OutputMessageText(string text) => Text = text;
        /// <summary>
        /// Creates a new instance of <see cref="OutputMessageText"/>, copied from <paramref name="other"/>.
        /// </summary>
        /// <param name="other">An instance to be copied.</param>
        public OutputMessageText(IOutputMessage other) : base(other) => Text = other.GetMessageText();

        /// <summary>
        /// Builds object's data and packs it into one text so it could be easily sent to server.
        /// </summary>
        /// <returns>Valid text, ready to be sent.</returns>
        public override string GetMessageText() => Text;

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone() => new OutputMessageText(this);
    }
}