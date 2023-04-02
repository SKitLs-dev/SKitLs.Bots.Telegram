using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.MessageUpdates;
using SKitLs.Bots.Telegram.Management.Managers.Prototype;

namespace SKitLs.Bots.Telegram.Management.AdvancedHandlers.Prototype
{
    /// <summary>
    /// Интерфейс классов-обработчиков входящих текстовых сообщений
    /// </summary>
    public interface ITextMessageUpdateHandler : IUpdateHandlerBase
    {
        /// <summary>
        /// Метод проверки текста на наличие команды
        /// </summary>
        public Func<string, bool> IsCommand { get; set; }
        /// <summary>
        /// Менеджер входящих команд
        /// </summary>
        public ICommandsManager CommandsManager { get; set; }
        /// <summary>
        /// Менеджер входящего текста
        /// </summary>
        public ITextInputManager TextInputManager { get; set; }

        /// <summary>
        /// Обработчик входящих текстовых сообщений
        /// </summary>
        /// <param name="update">Обновление текстового сообщения</param>
        public Task HandleUpdateAsync(SignedMessageTextUpdate update);
    }
}
