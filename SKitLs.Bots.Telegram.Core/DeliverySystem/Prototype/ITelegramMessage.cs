using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype
{
    /// <summary>
    /// An interface that defines the entity for sending messages to the
    /// <see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>.
    /// Use this interface to build text messages for sending.
    /// </summary>
    public interface ITelegramMessage : ICloneable
    {
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Represents the mode for parsing entities in the message text.
        /// <para/>
        /// See <see href="https://core.telegram.org/bots/api#formatting-options">formatting options</see> for more details.
        /// </summary>
        public ParseMode? ParseMode { get; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Represents the ID of the replied-to message, if the message is a reply.
        /// <para/>
        /// Use with <see cref="AllowSendingWithoutReply"/> to prevent from server's errors.
        /// </summary>
        public int? ReplyToMessageId { get; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Disables link previews for links in this message.
        /// </summary>
        public bool DisableWebPagePreview { get; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Enables <see href="https://telegram.org/blog/channels-2-0#silent-messages">silent</see> mode for a message.
        /// Users will receive a notification with no sound.
        /// </summary>
        public bool DisableNotification { get; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Enables protection for the contents of the sent message from forwarding and saving.
        /// </summary>
        public bool ProtectContent { get; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Indicates whether the message should be sent even if the specified via <see cref="ReplyToMessageId"/> replied-to message is not found.
        /// </summary>
        public bool AllowSendingWithoutReply { get; }

        /// <summary>
        /// Builds object's data and converts it to the text that could be easily sent to server.
        /// </summary>
        /// <returns>Text of the message to be sent, 1-4096 characters.</returns>
        public string GetMessageText();
        /// <summary>
        /// Generates message's reply markup that represents message's <see href="https://core.telegram.org/bots/features#keyboards">keyboard</see>.
        /// </summary>
        /// <returns>The <see cref="IReplyMarkup"/> instance.</returns>
        public IReplyMarkup? GetReplyMarkup();
    }
}