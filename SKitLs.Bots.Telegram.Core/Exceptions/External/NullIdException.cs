namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception that occurs when trying to add an item with a null or empty ID.
    /// </summary>
    public class NullIdException : SKTgSignedException
    {
        /// <summary>
        /// The type of object causing the exception.
        /// </summary>
        public Type TroubleMaker { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullIdException"/> class with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown the exception.</param>
        /// <param name="troubleMaker">The type of object causing the exception.</param>
        public NullIdException(object sender, Type troubleMaker)
            : base("NullOrEmptyId", SKTEOriginType.External, sender, troubleMaker.Name)
        {
            TroubleMaker = troubleMaker;
        }
    }
}