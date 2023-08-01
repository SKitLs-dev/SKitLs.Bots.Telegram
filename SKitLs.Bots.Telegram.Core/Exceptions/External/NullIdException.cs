namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception which occurs when trying to add an item with null or empty id.
    /// </summary>
    public class NullIdException : SKTgSignedException
    {
        /// <summary>
        /// Type of an object which has duplicated id.
        /// </summary>
        public Type TroubleMaker { get; private init; }

        /// <summary>
        /// Creates a new instance of <see cref="NullIdException"/> with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown exception.</param>
        /// <param name="troubleMaker">Type of an object which has duplicated id.</param>
        public NullIdException(object sender, Type troubleMaker)
            : base("NullOrEmptyId", SKTEOriginType.External, sender, troubleMaker.Name)
        {
            TroubleMaker = troubleMaker;
        }
    }
}