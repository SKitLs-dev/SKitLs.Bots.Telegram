using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public class CastedUpdate : ICastedUpdate
    {
        public BotManager Owner { get; set; }
        public Update OriginalSource { get; set; }
        public UpdateType Type => OriginalSource.Type;

        public ChatType ChatType { get; set; }
        public long ChatId { get; set; }

        public CastedUpdate(BotManager owner, Update source, ChatType chatType, long chatId)
        {
            OriginalSource = source;
            ChatType = chatType;
            ChatId = chatId;
            Owner = owner;
        }
    }
}