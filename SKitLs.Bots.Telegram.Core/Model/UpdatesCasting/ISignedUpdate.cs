using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public interface ISignedUpdate
    {
        public IBotUser Sender { get; }
    }
}