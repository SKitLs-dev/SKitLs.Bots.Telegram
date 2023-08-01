namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception which occurs on attempt of adding a new item with an existing id.
    /// </summary>
    public class DuplicationException : SKTgSignedException
    {
        /// <summary>
        /// Type of an object which has duplicated id.
        /// </summary>
        public Type TroubleMaker { get; init; }
        /// <summary>
        /// Additional information.
        /// </summary>
        public string Details { get; init; }

        /// <summary>
        /// Creates a new instance of <see cref="DuplicationException"/> with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown exception.</param>
        /// <param name="troubleMaker">Type of an object which has duplicated id.</param>
        /// <param name="details">Additional information.</param>
        public DuplicationException(Type sender, Type troubleMaker, string details)
            : base("Duplication", SKTEOriginType.External, sender, troubleMaker.Name, details)
        {
            TroubleMaker = troubleMaker;
            Details = details;
        }
    }
}