namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    public class MultiblockMessage : OutputMessage
    {
        public string? Header { get; set; }
        public List<string> Sections { get; set; } = new();
        public string? Footer { get; set; }

        public void AddBlock(string block) => Sections.Add(block);

        public override string GetMessageText()
        {
            string text = string.Empty;
            if (Header is not null) text += $"{Header}\n\n";
            foreach (string section in Sections)
                if (section is not null) text += $"{section}\n\n";
            if (Footer is not null) text += $"{Footer}";
            return text;
        }
        public override object Clone()
        {
            var _sec = new List<string>();
            Sections.ForEach(x => _sec.Add((string)x.Clone()));
            return new MultiblockMessage()
            {
                FormattedClone = FormattedClone,
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
