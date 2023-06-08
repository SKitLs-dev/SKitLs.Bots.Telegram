namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    public class DuplicationException : SKTgException
    {
        public Type Sender { get; set; }
        public Type TroubleMaker { get; set; }
        public string Details { get; set; }

        public DuplicationException(Type sender, Type troubleMaker, string details)
            : base("Duplication", SKTEOriginType.External, sender.Name, troubleMaker.Name, details)
        {
            Sender = sender;
            TroubleMaker = troubleMaker;
            Details = details;
        }
    }
}