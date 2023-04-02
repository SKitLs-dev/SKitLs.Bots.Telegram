using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Management.AdvancedHandlers.Prototype
{
    public interface ISignedMessageUpdateHandler : IUpdateHandlerBase
    {
        /// <summary>
        /// Обработчик входящих текстовых сообщений
        /// </summary>
        public ITextMessageUpdateHandler TextMessageUpdateHandler { get; set; }

        /// <summary>
        /// Обработчик входящих сообщений, делигирующий исполнение в зависимости от типа содержимого сообщения
        /// </summary>
        /// <param name="update">Входящее сообщение</param>
        public Task HandleUpdateAsync(SignedMessageUpdate update);
    }
}
