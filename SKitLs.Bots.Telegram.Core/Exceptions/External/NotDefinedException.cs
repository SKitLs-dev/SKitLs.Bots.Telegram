namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    public class NotDefinedException : SKTgException
    {
        public Type Sender { get; set; }
        public Type TroubleMaker { get; set; }
        public string Details { get; set; }

        public NotDefinedException(Type sender, Type troubleMaker, string details)
            : base("ItemNotDefined", SKTEOriginType.External, sender.Name, troubleMaker.Name, details)
        {
            Sender = sender;
            TroubleMaker = troubleMaker;
            Details = details;
        }
    }
}