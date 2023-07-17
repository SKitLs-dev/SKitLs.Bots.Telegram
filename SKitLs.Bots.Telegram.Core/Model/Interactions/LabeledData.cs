namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    public class LabeledData
    {
        public string Label { get; private set; }
        public string Data { get; private set; }

        public LabeledData(string label, string data)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}