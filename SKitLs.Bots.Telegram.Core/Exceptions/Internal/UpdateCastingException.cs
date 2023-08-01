namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    /// <summary>
    /// An exception which occurs when an update was not casted properly.
    /// </summary>
    public class UpdateCastingException : SKTgException
    {
        /// <summary>
        /// An id of the update that has thrown exception.
        /// </summary>
        public long UpdateId { get; private init; }
        /// <summary>
        /// Short displayable message of the update that has raised an excepion.
        /// </summary>
        public string UpdateName { get; private init; }

        /// <summary>
        /// Creates a new instance of <see cref="UpdateCastingException"/> with specified data.
        /// </summary>
        /// <param name="updateId">An id of the update that has thrown exception.</param>
        /// <param name="updateName">Short displayable message of the update that has raised an excepion.</param>
        public UpdateCastingException(long updateId, string updateName) : base("UpdateCasting", SKTEOriginType.Internal, updateId.ToString(), updateName)
        {
            UpdateId = updateId;
            UpdateName = updateName;
        }
    }
}