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
        public BotManager Owner => ChatScanner.Owner;

        public ChatScanner ChatScanner { get; private set; }
        public ChatType ChatType => ChatScanner.ChatType;
        public long ChatId { get; private set; }

        public Update OriginalSource { get; private set; }
        public UpdateType Type => OriginalSource.Type;

        /// <summary>
        /// Creates a new instance of an <see cref="CastedUpdate"/>, using specific data.
        /// </summary>
        /// <param name="chatScanner">Chat Scanner that has raised casted update</param>
        /// <param name="source">Original telegram update. Not casted, contains null values</param>
        /// <param name="chatId">ID of a chat that has raised updated</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CastedUpdate(ChatScanner chatScanner, Update source, long chatId)
        {
            ChatScanner = chatScanner ?? throw new ArgumentNullException(nameof(chatScanner));
            OriginalSource = source ?? throw new ArgumentNullException(nameof(source));
            ChatId = chatId;
        }
    }
}