using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text
{
    /// <summary>
    /// Specific <see cref="OutputMessage"/> that provides fancy text, consisted of text blocks.
    /// Represents an advanced Text Message that can be processed by <see cref="AdvancedDeliverySystem"/>.
    /// </summary>
    public class MultiblockMessage : OutputMessage
    {
        /// <summary>
        /// Special message's leading block. Marked as Bold.
        /// </summary>
        public string? Header { get; set; }
        /// <summary>
        /// Message's text blocks. Separated with double '\n' from each other.
        /// </summary>
        public List<string> Sections { get; set; } = new();
        /// <summary>
        /// Special message's closing block. Marked as Italic.
        /// </summary>
        public string? Footer { get; set; }

        /// <summary>
        /// Adds new section block to message's interior.
        /// </summary>
        /// <param name="block"></param>
        public void AddBlock(string block) => Sections.Add(block);

        /// <summary>
        /// Builds object's data and packs it into one text so it could be easily sent to server.
        /// </summary>
        /// <returns>Valid text, ready to be sent.</returns>
        public override string GetMessageText()
        {
            string text = string.Empty;
            if (Header is not null) text += $"*{Header}*\n\n";
            foreach (string section in Sections)
                if (section is not null) text += $"{section}\n\n";
            if (Footer is not null) text += $"_{Footer}_";
            return text;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            var _sec = new List<string>();
            Sections.ForEach(x => _sec.Add((string)x.Clone()));
            return new MultiblockMessage()
            {
                ReplyToMessageId = ReplyToMessageId,
                Header = (string?)Header?.Clone(),
                Sections = _sec,
                Footer = (string?)Footer?.Clone(),
                ParseMode = ParseMode,
                Menu = Menu
            };
        }
    }
}
