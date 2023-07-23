namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception which occurs on attempt of adding a new item with an existing id.
    /// </summary>
    public class DuplicationException : SKTgException
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
        /// Additional information.
        /// </summary>
        public string Details { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="DuplicationException"/> with specified data.
        /// </summary>
        /// <param name="sender">Type of an object that has thrown an exception.</param>
        /// <param name="troubleMaker">Type of an object which has duplicated id.</param>
        /// <param name="details">Additional information.</param>
        public DuplicationException(Type sender, Type troubleMaker, string details)
            : base("Duplication", SKTEOriginType.External, sender.Name, troubleMaker.Name, details)
        {
            Sender = sender;
            TroubleMaker = troubleMaker;
            Details = details;
        }
    }
}