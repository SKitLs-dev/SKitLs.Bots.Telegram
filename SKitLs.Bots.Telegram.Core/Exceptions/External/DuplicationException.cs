namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// Exception thrown when attempting to add a new item with an existing ID.
    /// </summary>
    public class DuplicationException : SKTgSignedException
    {
        /// <summary>
        /// The type of object causing the duplicated ID.
        /// </summary>
        public Type TroubleMaker { get; init; }

        /// <summary>
        /// Additional information about the exception.
        /// </summary>
        public string Details { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicationException"/> class with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown the exception.</param>
        /// <param name="troubleMaker">The type of object causing the duplicated ID.</param>
        /// <param name="details">Additional information about the exception.</param>
        public DuplicationException(Type sender, Type troubleMaker, string details)
            : base("Duplication", SKTEOriginType.External, sender, troubleMaker.Name, details)
        {
            TroubleMaker = troubleMaker;
            Details = details;
        }
    }
}
