namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    public class OutputMessageDecorText : OutputMessageText
    {
        public string? Header { get; set; }
        public string? Footer { get; set; }

        public OutputMessageDecorText(string text) : base(text) { }

        public OutputMessageDecorText AddHeader(string header) { Header = header; return this; }
        public OutputMessageDecorText AddFooter(string footer) { Footer = footer; return this; }

        public override string GetMessageText()
        {
            string text = string.Empty;
            if (Header != null) text += $"{Header}\n\n";
            text += $"{Text}\n\n";
            if (Footer != null) text += $"{Footer}";
            return text;
        }

        public override object Clone() => new OutputMessageDecorText((string)Text.Clone())
        {
            FormattedClone = FormattedClone,
            ReplyToMessageId = ReplyToMessageId,
            Header = (string?)Header?.Clone(),
            Footer = (string?)Footer?.Clone(),
            ParseMode = ParseMode,
            Menu = Menu
        };
    }

    //public class OutputMessageDecorTextEdit : OutputMessageDecorText, IEditWrapper
    //{
    //    public int EditMessageId { get; set; }

    //    public OutputMessageDecorTextEdit(int eMID, string message) : base(message)
    //    {
    //        EditMessageId = eMID;
    //    }

    //    internal OutputMessageDecorTextEdit(OutputMessageDecorText @base, int eMID)
    //    {
    //        Header = @base.Header;
    //        Footer = @base.Footer;
    //        Sections = @base.Sections;
    //        EditMessageId = eMID;
    //    }

    //    public InlineKeyboardMarkup? InlineMarkup => (InlineKeyboardMarkup?)Markup ?? null;
    //}
}
