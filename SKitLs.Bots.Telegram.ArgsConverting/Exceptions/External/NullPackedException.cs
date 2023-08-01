using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.External
{
    /// <summary>
    /// Represents an exception that occurs when there is a failure to convert an object to a string via the
    /// <see cref="IArgsSerializeService.Pack{TIn}(TIn)"/> method.
    /// The exception is derived from the <see cref="ArgedInterException"/> class,
    /// which provides a base for argumented interaction exceptions with extension support.
    /// </summary>
    public class NullPackedException : ArgedInterException
    {
        /// <summary>
        /// Type of an object which has triggered packing exception.
        /// </summary>
        public Type TroubleMaker { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullPackedException"/> class with the specified data.
        /// </summary>
        /// <param name="sender">The object that caused the exception.</param>
        /// <param name="troubleMaker">The type of the object that triggered the packing exception.</param>
        public NullPackedException(object sender, Type troubleMaker)
            : base("NullPacked", SKTEOriginType.External, sender, troubleMaker.Name) => TroubleMaker = troubleMaker;
    }
}