using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using Telegram.Bot;

namespace SKitLs.Bots.Telegram.Template.App
{
    /// <summary>
    /// Bot Commands Definition
    /// </summary>
    internal partial class MainApplicant
    {
        private void EnableCommands(IActionManager<SignedMessageTextUpdate> entity)
        {
            entity.AddSafely(StartCommand);
            entity.AddSafely(MenuCommand);
        }

        private static DefaultCommand StartCommand => new("start", Do_StartAsync);
        private static async Task Do_StartAsync(SignedMessageTextUpdate update)
        {
            var text = update.Owner.ResolveBotString("app.startUp", MenuCommand.ToString("C"));
            var startMessage = new OutputMessageText(text);

            var startMenu = new InlineMenu();
            startMenu.Add(ClickMeCallback);
            startMessage.Menu = startMenu;

            await update.Owner.DeliveryService.AnswerSenderAsync(await startMessage.BuildContentAsync(update), update);
        }

        private static DefaultCommand MenuCommand => new("menu", Do_MenuAsync);
        private static async Task Do_MenuAsync(SignedMessageTextUpdate update)
        {
            await update.Owner.DeliveryService.AnswerSenderAsync("Menu!", update);

            await OpenMainMenuAsync(update);
        }
    }
}
