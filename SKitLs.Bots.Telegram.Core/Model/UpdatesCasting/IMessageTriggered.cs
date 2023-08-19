using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    public interface IMessageTriggered
    {
        /// <summary>
        /// ID of a message that has raised current update.
        /// </summary>
        public int TriggerMessageId { get; }
    }
}