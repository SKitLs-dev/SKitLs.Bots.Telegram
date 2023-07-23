namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception which occurs when trying to add an item with null or empty id.
    /// </summary>
    public class NullIdException : SKTgException
    {
        /// <summary>
        /// Type of an object that has thrown an exception.
        /// </summary>
        public Type Sender { get; private set; }
        /// <summary>
        /// Type of an object which has duplicated id.
        /// </summary>
        public Type TroubleMaker { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="NullIdException"/> with specified data.
        /// </summary>
        /// <param name="sender">Type of an object that has thrown an exception.</param>
        /// <param name="troubleMaker">Type of an object which has duplicated id.</param>
        public NullIdException(Type sender, Type troubleMaker)
            : base("NullOrEmptyId", SKTEOriginType.External, sender.Name, troubleMaker.Name)
        {
            Sender = sender;
            TroubleMaker = troubleMaker;
        }
    }
}