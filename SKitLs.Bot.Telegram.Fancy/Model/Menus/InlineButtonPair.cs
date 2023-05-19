namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus
{
    public class InlineButtonPair
    {
        public string Label { get; set; }
        public string Data { get; set; }
        public bool SingleLine { get; set; }

        public InlineButtonPair(string label, string data)
        {
            Label = label;
            Data = data;
        }
    }
}