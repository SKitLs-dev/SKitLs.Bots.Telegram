namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    /// <summary>
    /// An enumeration that describes the origin of a thrown <see cref="SKTgException"/>.
    /// </summary>
    public enum SKTEOriginType
    {
        /// <summary>
        /// Represents an internal exception, indicating that the exception was thrown as a result of internal processes.
        /// For example, when some data was not defined.
        /// </summary>
        Internal = -10,

        /// <summary>
        /// Represents an exception that could be thrown by either internal processes or external actions.
        /// For example, when some property was not defined or was incorrectly overridden.
        /// </summary>
        Inexternal = 0,

        /// <summary>
        /// Represents an external exception, indicating that the exception was thrown as a result of external actions.
        /// For example, when a user tried to add a duplicate item.
        /// </summary>
        External = 10,
    }
}