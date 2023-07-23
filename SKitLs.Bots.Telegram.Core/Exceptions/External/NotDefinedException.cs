namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception which occurs when trying to resolve an item that does not exist.
    /// </summary>
    public class NotDefinedException : SKTgException
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
        /// Creates a new instance of <see cref="NotDefinedException"/> with specified data.
        /// </summary>
        /// <param name="sender">Type of an object that has thrown an exception.</param>
        /// <param name="troubleMaker">Type of an object which has duplicated id.</param>
        /// <param name="details">Additional information.</param>
        public NotDefinedException(Type sender, Type troubleMaker, string details)
            : base("ItemNotDefined", SKTEOriginType.External, sender.Name, troubleMaker.Name, details)
        {
            Sender = sender;
            TroubleMaker = troubleMaker;
            Details = details;
        }
    }
}