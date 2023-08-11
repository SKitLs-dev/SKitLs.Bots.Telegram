using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    [Obsolete($"Replaced with {nameof(IDynamicMessage)}", true)]
    public interface IFormattableMessage
    {
        public Func<IOutputMessage, ISignedUpdate, IOutputMessage>? FormattedClone { get; }
        public bool ShouldBeFormatted => FormattedClone != null;
    }
}