namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    /// <summary>
    /// An enum that describes the origin of thrown <see cref="SKTgException"/>.
    /// </summary>
    public enum SKTEOriginType
    {
        /// <summary>
        /// Represents an internal exception, which means that exception was thrown as a result of some internal processes.
        /// For example, some data was not defined.
        /// </summary>
        Internal = -10,
        /// <summary>
        /// Represents an exception, that could be thrown either by some internal processes or external actions.
        /// For example, some property was not defined or was overridden incorrectly.
        /// </summary>
        Inexternal = 0,
        /// <summary>
        /// Represents an external exception, which means that exception was thrown as a result of some external actions.
        /// For example, user tried to add somewhat duplicate.
        /// </summary>
        External = 10,
    }
}