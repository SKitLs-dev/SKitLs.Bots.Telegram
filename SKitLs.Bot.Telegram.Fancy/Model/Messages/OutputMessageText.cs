using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    public class OutputMessageText : OutputMessage
    {
        public string Text { get; set; }

        public OutputMessageText(string text)
        {
            Text = text;
        }
        public OutputMessageText(IOutputMessage other) : base(other)
        {
            Text = other.GetMessageText();
        }

        public override string GetMessageText() => Text;
        public override object Clone() => new OutputMessageText(this)
        {
            Text = (string)Text.Clone()
        };
    }
}