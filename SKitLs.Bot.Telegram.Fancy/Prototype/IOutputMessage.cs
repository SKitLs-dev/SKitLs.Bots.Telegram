using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    /// <summary>
    /// Provides mechanics of creating advanced messages that can be processed by <see cref="AdvancedDeliverySystem"/>.
    /// </summary>
    public interface IOutputMessage : IBuildableMessage
    {
        /// <summary>
        /// Determines message id that current message should reply to.
        /// </summary>
        public int ReplyToMessageId { get; }

        /// <summary>
        /// Determines message's parse mode.
        /// </summary>
        public ParseMode? ParseMode { get; set; }
        /// <summary>
        /// Determines message's menu.
        /// </summary>
        public IMesMenu? Menu { get; set; }
    }
}