namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    public class NullIdException : SKTgException
    {
        public Type Sender { get; private set; }
        public Type TroubleMaker { get; private set; }

        public NullIdException(Type sender, Type troubleMaker)
            : base("NullOrEmptyId", SKTEOriginType.External, sender.Name, troubleMaker.Name)
        {
            Sender = sender;
            TroubleMaker = troubleMaker;
        }
    }
}