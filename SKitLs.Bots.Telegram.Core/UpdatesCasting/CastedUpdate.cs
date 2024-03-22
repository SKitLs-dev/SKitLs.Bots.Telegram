using SKitLs.Bots.Telegram.Core.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    /// <summary>
    /// Default implementation of the <see cref="ICastedUpdate"/> interface.
    /// Provides basic details of a raw server update. Acts as a wrapper for a raw <see cref="Update"/>.
    /// </summary>
    public class CastedUpdate : ICastedUpdate
    {
        /// <inheritdoc/>
        public BotManager Owner => ChatScanner.Owner;

        /// <inheritdoc/>
        public ChatScanner ChatScanner { get; init; }

        /// <inheritdoc/>
        public ChatType ChatType => ChatScanner.ChatType;

        /// <inheritdoc/>
        public long ChatId { get; init; }

        /// <inheritdoc/>
        public Update OriginalSource { get; init; }

        /// <inheritdoc/>
        public UpdateType Type => OriginalSource.Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastedUpdate"/> class with the specified data.
        /// </summary>
        /// <param name="chatScanner">The Chat Scanner that raised the casted update.</param>
        /// <param name="source">The original Telegram update. Not casted, may contain null values.</param>
        /// <param name="chatId">The ID of the chat that raised the update.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CastedUpdate(ChatScanner chatScanner, Update source, long chatId)
        {
            ChatScanner = chatScanner ?? throw new ArgumentNullException(nameof(chatScanner));
            OriginalSource = source ?? throw new ArgumentNullException(nameof(source));
            ChatId = chatId;
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Enum.GetName(ChatType)} ({OriginalSource.Id})";
    }
}