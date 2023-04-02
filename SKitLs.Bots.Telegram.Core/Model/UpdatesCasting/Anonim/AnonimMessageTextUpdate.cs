using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim
{
    public class AnonimMessageTextUpdate : AnonimMessageUpdate
    {
        public string Text { get; set; }

        public AnonimMessageTextUpdate(SignedMessageUpdate update) : base(update)
        {
            if (Message.Text is null)
                throw new UpdateCastingException("Anonim Text Message", update.OriginalSource.Id);
            
            Text = Message.Text;
        }
    }
}