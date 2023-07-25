namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text
{
    [Obsolete("Will be rebuilt")]
    public class OMDText : OutputMessageText
    {
        public string? Header { get; set; }
        public string? Footer { get; set; }

        public OMDText(string text) : base(text) { }

        public OMDText AddHeader(string header) { Header = header; return this; }
        public OMDText AddFooter(string footer) { Footer = footer; return this; }

        public override string GetMessageText()
        {
            string text = string.Empty;
            if (Header != null) text += $"{Header}\n\n";
            text += $"{Text}\n\n";
            if (Footer != null) text += $"{Footer}";
            return text;
        }

        public override object Clone() => new OMDText((string)Text.Clone())
        {
            ReplyToMessageId = ReplyToMessageId,
            Header = (string?)Header?.Clone(),
            Footer = (string?)Footer?.Clone(),
            ParseMode = ParseMode,
            Menu = Menu
        };
    }
}
