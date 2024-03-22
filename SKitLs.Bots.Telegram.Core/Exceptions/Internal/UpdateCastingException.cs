namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    /// <summary>
    /// An exception that occurs when an update fails to be casted properly.
    /// </summary>
    public class UpdateCastingException : SKTgException
    {
        /// <summary>
        /// The ID of the update that has thrown the exception.
        /// </summary>
        public long UpdateId { get; private init; }

        /// <summary>
        /// A short displayable message of the update that has raised an exception.
        /// </summary>
        public string UpdateName { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCastingException"/> class with specified data.
        /// </summary>
        /// <param name="updateId">The ID of the update that has thrown the exception.</param>
        /// <param name="updateName">A short displayable message of the update that has raised an exception.</param>
        public UpdateCastingException(long updateId, string updateName) : base("UpdateCasting", SKTEOriginType.Internal, updateId.ToString(), updateName)
        {
            UpdateId = updateId;
            UpdateName = updateName;
        }
    }
}