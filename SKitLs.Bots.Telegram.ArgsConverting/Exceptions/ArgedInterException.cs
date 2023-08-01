using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs when an error is related to <c>*.ArgedInteractions</c> framework extension.
    /// The exception is derived from the <see cref="SKTgSignedException"/> class, which provides a base for
    /// signed exceptions in the application.
    /// </summary>
    public class ArgedInterException : SKTgSignedException
    {
        /// <summary>
        /// Gets or sets the extension prefix used for localization keys.
        /// </summary>
        public static string ExtensionLocalPrefix { get; set; } = "ai";

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgedInterException"/> class with the specified parameters.
        /// </summary>
        /// <param name="localKey">A string representing the local key for the exception message.</param>
        /// <param name="originType">The origin type of the exception.</param>
        /// <param name="sender">The object that caused the exception.</param>
        /// <param name="format">Optional. Array of format strings used for the exception message.</param>
        public ArgedInterException(string localKey, SKTEOriginType originType, object sender, params string?[] format)
            : base($"{ExtensionLocalPrefix}.{localKey}", originType, sender, format) { }
    }
}