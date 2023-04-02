using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    public class SignedMessageTextUpdate : AnonimMessageUpdate
    {
        public string Text { get; set; }

        public SignedMessageTextUpdate(SignedMessageUpdate update) : base(update)
        {
            if (Message.Text is null)
                throw new UpdateCastingException("Signed Text Message", update.OriginalSource.Id);
            
            Text = Message.Text;
        }
    }
}