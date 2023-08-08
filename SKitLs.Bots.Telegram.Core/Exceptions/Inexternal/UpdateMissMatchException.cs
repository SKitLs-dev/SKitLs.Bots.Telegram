namespace SKitLs.Bots.Telegram.Core.Exceptions.Inexternal
{
    // XML-Doc Update
    /// <summary>
    /// Represents an exception that occurs when there is a mismatch in update processing.
    /// </summary>
    public class UpdateMissMatchException : SKTgSignedException
    {
        /// <summary>
        /// Type of an update that was expected.
        /// </summary>
        public Type ExpectedType { get; private init; }

        /// <summary>
        /// Type of an update that was received.
        /// </summary>
        public Type ReceivedType { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMissMatchException"/> class with the specified sender object.
        /// </summary>
        /// <param name="sender">The object that has thrown exception.</param>
        /// <param name="expectedType">Type of an update that was expected.</param>
        /// <param name="receivedType">Type of an update that was received.</param>
        public UpdateMissMatchException(object sender, Type expectedType, Type receivedType)
            : base("UpdateMissMatch", SKTEOriginType.Inexternal, sender, expectedType.Name, receivedType.Name)
        {
            ExpectedType = expectedType;
            ReceivedType = receivedType;
        }
    }
}