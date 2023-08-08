using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    // XML-Doc Update
    /// <summary>
    /// Provides basic detalization of a raw server's update. Wrapper of a raw <see cref="Update"/>.
    /// </summary>
    public interface ICastedUpdate
    {
        /// <summary>
        /// Bot Manager that has raised casted update.
        /// </summary>
        public BotManager Owner { get; }
        /// <summary>
        /// Chat Scanner that has raised casted update.
        /// </summary>
        public ChatScanner ChatScanner { get; }
        /// <summary>
        /// <see cref="ChatScanner"/>'s type.
        /// </summary>
        public ChatType ChatType { get; }
        /// <summary>
        /// ID of a chat that has raised updated.
        /// </summary>
        public long ChatId { get; }
        /// <summary>
        /// Original telegram update. Not casted, contains null values.
        /// </summary>
        public Update OriginalSource { get; }
        /// <summary>
        /// <see cref="OriginalSource"/> update's type.
        /// </summary>
        public UpdateType Type { get; }
    }
}