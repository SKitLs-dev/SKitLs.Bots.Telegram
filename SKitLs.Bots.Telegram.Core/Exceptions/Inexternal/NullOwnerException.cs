using SKitLs.Bots.Telegram.Core.Model.Building;

namespace SKitLs.Bots.Telegram.Core.Exceptions.Inexternal
{
    /// <summary>
    /// An exception which occurs when <see cref="IOwnerCompilable"/> class has not determined its owner
    /// during reflective compilation.
    /// </summary>
    public class NullOwnerException : SKTgSignedException
    {
        /// <summary>
        /// Creates a new instance of <see cref="NullOwnerException"/> with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown exception.</param>
        public NullOwnerException(object sender) : base("NullOwner", SKTEOriginType.Inexternal, sender) { }
    }
}