namespace SKitLs.Bots.Telegram.DataBases.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ShotLabeledAttribute : Attribute
    {
        public string Label { get; set; }

        public ShotLabeledAttribute(string label) => Label = label ?? throw new ArgumentNullException(nameof(label));
    }
}