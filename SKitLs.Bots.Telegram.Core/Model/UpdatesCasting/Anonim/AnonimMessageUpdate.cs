using SKitLs.Bots.Telegram.Core.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim
{
    public class AnonimMessageUpdate : CastedUpdate
    {
        public Message Message { get; set; }

        public AnonimMessageUpdate(BotManager owner, Update source, ChatType chatType, long chatId)
            : base(owner, source, chatType, chatId)
        {
            if (source.Message is null)
                throw new UpdateCastingException("Anonim Message", source.Id);
            
            Message = source.Message;
        }
        public AnonimMessageUpdate(CastedUpdate update)
            : this(update.Owner, update.OriginalSource, update.ChatType, update.ChatId)
        { }
    }
}
