using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public interface ISignedUpdate
    {
        public IBotUser Sender { get; }
    }
}