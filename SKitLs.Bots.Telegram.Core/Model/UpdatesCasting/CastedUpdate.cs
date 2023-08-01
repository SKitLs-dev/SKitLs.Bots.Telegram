using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    /// <summary>
    /// Default realization of an <see cref="ICastedUpdate"/>.
    /// Provides basic detalization of a raw server's update. Wrapper of a raw <see cref="Update"/>.
    /// </summary>
    public class CastedUpdate : ICastedUpdate
    {
        /// <summary>
        /// Bot Manager that has raised casted update.
        /// </summary>
        public BotManager Owner => ChatScanner.Owner;

        /// <summary>
        /// Chat Scanner that has raised casted update.
        /// </summary>
        public ChatScanner ChatScanner { get; init; }
        /// <summary>
        /// <see cref="ChatScanner"/>'s type.
        /// </summary>
        public ChatType ChatType => ChatScanner.ChatType;
        /// <summary>
        /// ID of a chat that has raised updated.
        /// </summary>
        public long ChatId { get; init; }

        /// <summary>
        /// Original telegram update. Not casted, contains null values.
        /// </summary>
        public Update OriginalSource { get; init; }
        /// <summary>
        /// <see cref="OriginalSource"/> update's type.
        /// </summary>
        public UpdateType Type => OriginalSource.Type;

        /// <summary>
        /// Creates a new instance of an <see cref="CastedUpdate"/> with specified data.
        /// </summary>
        /// <param name="chatScanner">Chat Scanner that has raised casted update.</param>
        /// <param name="source">Original telegram update. Not casted, contains null values.</param>
        /// <param name="chatId">ID of a chat that has raised updated.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CastedUpdate(ChatScanner chatScanner, Update source, long chatId)
        {
            ChatScanner = chatScanner ?? throw new ArgumentNullException(nameof(chatScanner));
            OriginalSource = source ?? throw new ArgumentNullException(nameof(source));
            ChatId = chatId;
        }

        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override string ToString() => $"{Enum.GetName(ChatType)} ({OriginalSource.Id})";
    }
}