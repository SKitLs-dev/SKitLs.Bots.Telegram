using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.MessageUpdates
{
    public class MessageTextUpdate : MessageUpdate
    {
        public string Text { get; set; }

        public MessageTextUpdate(SignedMessageUpdate update) : base(update)
        {
            if (update.Message.Type != MessageType.Text || Message.Text == null)
                throw new ArgumentException("Ожидалось сообщение с текстом");
            Text = Message.Text;
        }
    }
}
