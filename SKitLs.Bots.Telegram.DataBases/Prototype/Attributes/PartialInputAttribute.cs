namespace SKitLs.Bots.Telegram.DataBases.Prototype.Attributes
{
    public abstract class BotInputPropAttribute : Attribute
    {
        public int DetalizationId { get; set; }
        public int Order { get; set; }

        public BotInputPropAttribute(int detalizationId, int order)
        {
            DetalizationId = detalizationId;
            Order = order;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class PartialInputAttribute : BotInputPropAttribute
    {
        public string? TextBuilderMethod { get; set; }
        public string? PreviewMethod { get; set; }
        public string? ParserMethod { get; set; }

        public PartialInputAttribute(int detalizationId, int order) : this(detalizationId, order, null) { }
        public PartialInputAttribute(int detalizationId, int order, string? parserName, string? previewName = null, string? textBuilderMethod = null) : base(detalizationId, order)
        {
            ParserMethod = parserName;
            PreviewMethod = previewName;
            TextBuilderMethod = textBuilderMethod;
        }
    }

    public class SelectorInputAttribute : BotInputPropAttribute
    {
        public string ValuesSelectorName { get; set; }
        public string? Mask { get; set; }

        public SelectorInputAttribute(int detalizationId, int order, string valuesSelectorName)
            : this(detalizationId, order, valuesSelectorName, null) { }
        public SelectorInputAttribute(int detalizationId, int order, string valuesSelectorName, string? mask) : base(detalizationId, order)
        {
            Mask = mask;
            ValuesSelectorName = valuesSelectorName;
        }
    }
}