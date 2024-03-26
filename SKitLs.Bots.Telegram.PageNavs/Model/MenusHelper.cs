using SKitLs.Bots.Telegram.AdvancedMessages.Editors;
using SKitLs.Bots.Telegram.AdvancedMessages.Messages.Text;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Settings;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// Helper class providing methods for handling menu-related operations.
    /// </summary>
    public static class MenusHelper
    {
        /// <summary>
        /// Handles errors when resolving <see cref="PageSessionData"/> or throws an unexpected exception.
        /// Blocks the inline menu of the <paramref name="update"/>'s message page by removing it and notifies the sender that the session has expired.
        /// <para/>
        /// Notification content can be overridden via <see cref="BotManager.Localizator"/> with the <see cref="PNSettings.SessionExpiredLocalKey"/> local key.
        /// </summary>
        /// <param name="update">The update that has raised an exception.</param>
        public static async Task HandleSessionExpiredAsync(SignedCallbackUpdate update)
        {
            var mes = new LocalizedTextMessage(PNSettings.SessionExpiredLocalKey).Edit(update);
            await update.Owner.DeliveryService.AnswerSenderAsync(mes, update);
        }
    }
}