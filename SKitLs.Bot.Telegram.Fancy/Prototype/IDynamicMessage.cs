using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    public interface IDynamicMessage
    {
        public Func<ISignedUpdate?, IOutputMessage> MessageBuilder { get; }
        public IOutputMessage BuildWith(ISignedUpdate? update);
    }
}