namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception which occurs when trying to resolve an item that does not exist.
    /// </summary>
    public class NotDefinedException : SKTgSignedException
    {
        /// <summary>
        /// Type of an object which has duplicated id.
        /// </summary>
        public Type TroubleMaker { get; private init; }
        /// <summary>
        /// Additional information.
        /// </summary>
        public string Details { get; private init; }

        /// <summary>
        /// Creates a new instance of <see cref="NotDefinedException"/> with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown exception.</param>
        /// <param name="troubleMaker">Type of an object which has duplicated id.</param>
        /// <param name="details">Additional information.</param>
        public NotDefinedException(object sender, Type troubleMaker, string details)
            : base("ItemNotDefined", SKTEOriginType.External, sender, troubleMaker.Name, details)
        {
            TroubleMaker = troubleMaker;
            Details = details;
        }
    }
}