using SKitLs.Bots.Telegram.Core.Model.Building;

namespace SKitLs.Bots.Telegram.Core.Exceptions.Inexternal
{
    /// <summary>
    /// An exception that occurs when a class implementing <see cref="IOwnerCompilable"/>
    /// has not determined its owner during reflective compilation.
    /// </summary>
    public class NullOwnerException : SKTgSignedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullOwnerException"/> class with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown the exception.</param>
        public NullOwnerException(object sender) : base("NullOwner", SKTEOriginType.Inexternal, sender) { }
    }
}