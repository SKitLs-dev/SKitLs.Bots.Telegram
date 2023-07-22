using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    public interface IFormattableMessage
    {
        public Func<IOutputMessage, ISignedUpdate, IOutputMessage>? FormattedClone { get; }
        public bool ShouldBeFormatted => FormattedClone != null;
    }
}