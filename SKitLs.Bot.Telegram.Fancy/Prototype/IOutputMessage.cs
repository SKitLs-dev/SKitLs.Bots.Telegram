using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    /// <summary>
    /// Represents a message interface designed for creating advanced messages that can be processed by the <see cref="AdvancedDeliverySystem"/>.
    /// </summary>
    public interface IOutputMessage : IBuildableMessage
    {
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Represents the mode for parsing entities in the message text.
        /// <para/>
        /// See <see href="https://core.telegram.org/bots/api#formatting-options">formatting options</see> for more details.
        /// </summary>
        public ParseMode ParseMode { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Represents the ID of the replied-to message, if the message is a reply.
        /// <para/>
        /// Use with <see cref="AllowSendingWithoutReply"/> to prevent from server's errors.
        /// </summary>
        public int ReplyToMessageId { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Disables link previews for links in this message.
        /// </summary>
        public bool DisableWebPagePreview { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Enables <see href="https://telegram.org/blog/channels-2-0#silent-messages">silent</see> mode for a message.
        /// Users will receive a notification with no sound.
        /// </summary>
        public bool DisableNotification { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Enables protection for the contents of the sent message from forwarding and saving.
        /// </summary>
        public bool ProtectContent { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#sendmessage">Telegram API</see>]</b>
        /// Indicates whether the message should be sent even if the specified via <see cref="ReplyToMessageId"/> replied-to message is not found.
        /// </summary>
        public bool AllowSendingWithoutReply { get; set; }

        /// <summary>
        /// Represents the message's menu, if applicable.
        /// </summary>
        public IBuildableContent<IMessageMenu>? Menu { get; set; }
    }
}