using SKitLs.Bots.Telegram.Core.Exceptions;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.Inexternal
{
    /// <summary>
    /// Represents an exception that occurs when attempting to convert a <see langword="null"/> input during argumented interactions.
    /// The exception is derived from the <see cref="ArgedInterException"/> class,
    /// which provides a base for argumented interaction exceptions with extension support.
    /// </summary>
    public class ConvertNullInputException : ArgedInterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertNullInputException"/> class with the specified sender.
        /// </summary>
        /// <param name="sender">The object that caused the exception.</param>
        public ConvertNullInputException(object sender) : base("ConvertNullInput", SKTEOriginType.Inexternal, sender)
        { }
    }
}