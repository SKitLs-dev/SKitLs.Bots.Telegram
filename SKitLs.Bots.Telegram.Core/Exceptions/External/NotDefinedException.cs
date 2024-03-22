namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception that occurs when trying to resolve an item that does not exist.
    /// </summary>
    public class NotDefinedException : SKTgSignedException
    {
        /// <summary>
        /// The type of object causing the exception.
        /// </summary>
        public Type TroubleMaker { get; private init; }

        /// <summary>
        /// Additional information about the exception.
        /// </summary>
        public string Details { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotDefinedException"/> class with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown the exception.</param>
        /// <param name="troubleMaker">The type of object causing the exception.</param>
        /// <param name="details">Additional information about the exception.</param>
        public NotDefinedException(object sender, Type troubleMaker, string details)
            : base("ItemNotDefined", SKTEOriginType.External, sender, troubleMaker.Name, details)
        {
            TroubleMaker = troubleMaker;
            Details = details;
        }
    }
}