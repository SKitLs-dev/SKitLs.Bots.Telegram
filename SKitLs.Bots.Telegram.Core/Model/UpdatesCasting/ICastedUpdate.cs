using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public interface ICastedUpdate
    {
        public BotManager Owner { get; }
        public Update OriginalSource { get; }
        public UpdateType Type => OriginalSource.Type;

        public ChatType ChatType { get; }
        public long ChatId { get; }
    }
}