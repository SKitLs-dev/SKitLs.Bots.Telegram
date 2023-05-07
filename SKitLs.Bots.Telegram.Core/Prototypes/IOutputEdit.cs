namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IOutputEdit : IOutputMessage
    {
        public int EditMessageId { get; }
    }
}