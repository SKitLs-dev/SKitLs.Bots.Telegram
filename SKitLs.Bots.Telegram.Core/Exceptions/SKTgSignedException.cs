using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    /// <summary>
    /// Represents a base for project's exceptions. Contains information about exception's origin,
    /// its description localization keys and sender data.
    /// </summary>
    public class SKTgSignedException : SKTgException
    {
        /// <summary>
        /// Determines the object that has thrown exception.
        /// </summary>
        public object Sender { get; private init; }
        /// <summary>
        /// Type of a class that has thrown an exception.
        /// </summary>
        public Type SenderType => Sender.GetType();

        /// <summary>
        /// Initializes a new instance of the <see cref="SKTgSignedException"/> class with the specified sender object.
        /// </summary>
        /// <param name="localKey">Key base for resolving localization string.</param>
        /// <param name="originType">The origin of thrown exception.</param>
        /// <param name="sender">The object that has thrown exception.</param>
        /// <param name="format">Additional strings that should be written in localized exception message.</param>
        public SKTgSignedException(string localKey, SKTEOriginType originType, object sender, params string?[] format) : base(localKey, originType, Concat(sender, format))
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