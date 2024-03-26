using SKitLs.Bots.Telegram.AdvancedMessages.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.Core.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;

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
            await new LocalizedTextMessage("app.startUp", MenuCommand.ToString("C"))
            {
                Menu = new InlineMenu(true, 2)
                {
                    { ClickMeCallback, true },
                    { ClickMeCallback, false },
                    { ClickMeCallback, false },
                }
            }.AnswerAsync(update);
        }

        private static DefaultCommand MenuCommand => new("menu", Do_MenuAsync);
        private static async Task Do_MenuAsync(SignedMessageTextUpdate update)
        {
            await update.Owner.DeliveryService.AnswerSenderAsync("Menu!", update);
            await OpenMainMenuAsync(update);
        }
    }
}
