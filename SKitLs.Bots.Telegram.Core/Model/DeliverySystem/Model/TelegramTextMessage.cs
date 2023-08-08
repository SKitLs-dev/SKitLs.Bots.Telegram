using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model
{
    /// <summary>
    /// Represents the default implementation of <see cref="ITelegramMessage"/> for sending text messages via
    /// the <see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>..
    /// </summary>
    public class TelegramTextMessage : ITelegramMessage
    {
        /// <inheritdoc/>
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc/>
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc/>
        public bool DisableWebPagePreview { get; set; }

        /// <inheritdoc/>
        public bool DisableNotification { get; set; }

        /// <inheritdoc/>
        public bool ProtectContent { get; set; }

        /// <inheritdoc/>
        public bool AllowSendingWithoutReply { get; set; }

        private string _text = null!;
        /// <summary>
        /// Gets or sets the text to be sent. Must be 1-4096 characters.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentNullException"/>
        public virtual string Text
        {
            get => _text;
            set
            {
                if (value.Length < 1 || value.Length > 4096)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _text = string.IsNullOrEmpty(value)
                    ? throw new ArgumentNullException(nameof(value))
                    : value;
            }
        }

        /// <summary>
        /// Gets or sets the reply markup associated with the message.
        /// </summary>
        public IReplyMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramTextMessage"/> class with default settings.
        /// </summary>
        protected TelegramTextMessage() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramTextMessage"/> class with the specified text.
        /// </summary>
        /// <param name="text">The text to be included in the message. Must be 1-4096 characters.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided text length is not within the allowed range.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided text is null or empty.</exception>
        public TelegramTextMessage(string text) => Text = text;
        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramTextMessage"/> class using properties from another <see cref="ITelegramMessage"/>.
        /// </summary>
        /// <param name="other">Another instance of <see cref="ITelegramMessage"/> to copy properties from.</param>
        public TelegramTextMessage(ITelegramMessage other)
        {
            ParseMode = other.ParseMode;
            ReplyToMessageId = other.ReplyToMessageId;
            DisableWebPagePreview = other.DisableWebPagePreview;
            DisableNotification = other.DisableNotification;
            ProtectContent = other.ProtectContent;
            AllowSendingWithoutReply = other.AllowSendingWithoutReply;
            Text = other.GetMessageText();
            ReplyMarkup = other.GetReplyMarkup();
        }

        /// <inheritdoc/>
        public string GetMessageText() => Text;

        /// <inheritdoc/>
        public IReplyMarkup? GetReplyMarkup() => ReplyMarkup;

        /// <inheritdoc/>
        public object Clone() => new TelegramTextMessage((string)Text.Clone());

        /// <inheritdoc/>
        public override string? ToString() => $"{GetType().Name} \"{Text}\"";
    }
}