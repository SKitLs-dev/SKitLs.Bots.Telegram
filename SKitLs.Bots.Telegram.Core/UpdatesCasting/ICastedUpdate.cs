using SKitLs.Bots.Telegram.Core.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.UpdatesCasting
{
    /// <summary>
    /// Provides basic details of a raw server update. Acts as a wrapper for a raw <see cref="Update"/>.
    /// </summary>
    public interface ICastedUpdate
    {
        /// <summary>
        /// Gets the <see cref="BotManager"/> that raised the casted update.
        /// </summary>
        public BotManager Owner { get; }

        /// <summary>
        /// Gets the <see cref="ChatScanner"/> that raised the casted update.
        /// </summary>
        public ChatScanner ChatScanner { get; }

        /// <summary>
        /// Gets the type of the Chat Scanner.
        /// </summary>
        public ChatType ChatType { get; }

        /// <summary>
        /// Gets the ID of the chat that raised the update.
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// Gets the original Telegram update. This update is not casted and may contain null values.
        /// </summary>
        public Update OriginalSource { get; }

        /// <summary>
        /// Gets the type of the <see cref="OriginalSource"/> update.
        /// </summary>
        public UpdateType Type { get; }
    }
}