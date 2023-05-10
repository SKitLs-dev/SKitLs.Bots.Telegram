using SKitLs.Bots.Telegram.Core.Prototypes;
using TEnums = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    public class OutputMessageDecorText : OutputMessageText
    {
        public string? Header { get; set; }
        public List<string> Sections { get; set; }
        public string? Footer { get; set; }

        public OutputMessageDecorText()
        {
            Sections = new();
            ParseMode = TEnums.ParseMode.Markdown;
        }
        public OutputMessageDecorText(string? message)
        {
            Sections = new();
            ParseMode = TEnums.ParseMode.Markdown;
            if (message != null)
                Sections.Add(message);
        }
        public OutputMessageDecorText(IBotDisplayable data, int displayFullness)
        {
            Sections = new() { data.FullDisplay(displayFullness) };
        }
        public OutputMessageDecorText(List<IBotDisplayable> data, int start = 0, int size = 0, string? header = null)
        {
            Header = header;
            Sections = new();
            string list = string.Empty;
            if (start == 0 || size == 0)
                data.ForEach(x => list += $"• {x.ListDisplay()}\n");
            else if (data.Count > 0)
                for (int i = start; i < (start + size > data.Count ? data.Count : start + size); i++)
                    list += $"• {data[i].ListDisplay()}\n";
            Sections.Add(list);
        }
        //public OutputMessageDecorTextEdit AsEdit(int eMID) => new(this, eMID);
        public OutputMessageDecorText AddHeader(string header) { Header = header; return this; }
        public OutputMessageDecorText AddFooter(string footer) { Footer = footer; return this; }

        public override string GetMessageText()
        {
            string text = string.Empty;
            if (Header != null) text += $"{Header}\n\n";
            foreach (string section in Sections)
                if (section != null) text += $"{section}\n\n";
            if (Footer != null) text += $"{Footer}";
            return text;
        }

        public override object Clone()
        {
            var _sec = new List<string>();
            Sections.ForEach(x => _sec.Add((string)x.Clone()));
            return new OutputMessageDecorText()
            {
                Header = (string?)Header?.Clone(),
                Sections = _sec,
                Footer = (string?)Footer?.Clone(),
                ParseMode = ParseMode,
                Markup = Markup
            };
        }
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
