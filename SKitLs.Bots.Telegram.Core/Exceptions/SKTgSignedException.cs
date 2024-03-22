using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    /// <summary>
    /// Represents a base class for exceptions in the project, including information about the exception's origin,
    /// its description localization keys, and sender data.
    /// </summary>
    public class SKTgSignedException : SKTgException
    {
        /// <summary>
        /// Represents the object that has thrown the exception.
        /// </summary>
        public object Sender { get; private init; }

        /// <summary>
        /// Gets the type of the class that has thrown the exception.
        /// </summary>
        public Type SenderType => Sender.GetType();

        /// <summary>
        /// Initializes a new instance of the <see cref="SKTgSignedException"/> class with the specified sender object.
        /// </summary>
        /// <param name="localKey">The base key for resolving localization strings.</param>
        /// <param name="originType">The origin of the thrown exception.</param>
        /// <param name="sender">The object that has thrown the exception.</param>
        /// <param name="format">Additional strings that should be written in the localized exception message.</param>
        public SKTgSignedException(string localKey, SKTEOriginType originType, object sender, params string?[] format)
            : base(localKey, originType, Concat(sender, format))
        {
            Sender = sender;
        }

        private static string?[] Concat(object sender, string?[] args)
        {
            var res = new string?[args.Length + 1];
            res[0] = sender is IDebugNamed named ? named.DebugName : sender.GetType().Name;
            for (int i = 0; i < args.Length; i++)
                res[i + 1] = args[i];
            return res;
        }
    }
}