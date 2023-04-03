using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    public class SignedMessageTextUpdate : AnonimMessageTextUpdate, ISignedUpdate
    {
        public IBotUser Sender { get; set; }

        public SignedMessageTextUpdate(AnonimMessageTextUpdate update, IBotUser sender) : base(update)
        {
            if (Message.Text is null)
                throw new UpdateCastingException("Signed Text Message", update.OriginalSource.Id);
            
            Sender = sender;
            Text = Message.Text;
        }
        public SignedMessageTextUpdate(SignedMessageUpdate update) : base(update)
        {
            if (Message.Text is null)
                throw new UpdateCastingException("Signed Text Message", update.OriginalSource.Id);

            Sender = update.Sender;
            Text = Message.Text;
        }
    }
}