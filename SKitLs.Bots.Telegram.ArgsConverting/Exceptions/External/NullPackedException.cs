using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.External
{
    /// <summary>
    /// An exception which occurs when attempt to convert object to string via
    /// <see cref="IArgsSerilalizerService.Pack{TIn}(TIn)"/> was failed.
    /// </summary>
    public class NullPackedException : SKTgException
    {
        /// <summary>
        /// Type of an object which has triggered packing exception.
        /// </summary>
        public Type TroubleMaker { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="NullPackedException"/> with specified data.
        /// </summary>
        /// <param name="troubleMaker">Type of an object which has triggered packing exception.</param>
        public NullPackedException(Type troubleMaker) : base("NullPacked", SKTEOriginType.External, troubleMaker.Name)
            => TroubleMaker = troubleMaker;
    }
}