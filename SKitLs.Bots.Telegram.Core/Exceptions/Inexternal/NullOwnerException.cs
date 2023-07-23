using SKitLs.Bots.Telegram.Core.Model.Building;

namespace SKitLs.Bots.Telegram.Core.Exceptions.Inexternal
{
    /// <summary>
    /// An exception which occurs when <see cref="IOwnerCompilable"/> class has not determined its owner
    /// during reflective compilation.
    /// </summary>
    public class NullOwnerException : SKTgException
    {
        /// <summary>
        /// Type of a class that has thrown an exception.
        /// </summary>
        public Type SenderType { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="NullOwnerException"/> with specified data.
        /// </summary>
        /// <param name="senderType">Type of a class that has thrown an exception.</param>
        public NullOwnerException(Type senderType) : base("NullOwner", SKTEOriginType.Inexternal, senderType.Name)
        {
            SenderType = senderType;
        }
    }
}