using SKitLs.Bots.Telegram.AdvancedMessages.Buttons.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Editors;
using SKitLs.Bots.Telegram.AdvancedMessages.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.Core.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Template.App
{
    internal partial class MainApplicant
    {
        private void EnableCallbacks(IActionManager<SignedCallbackUpdate> entity)
        {
            entity.AddSafely(ClickMeCallback);
        }

        public static DefaultCallback ClickMeCallback { get; } = new("ClickMe", "app.JoinUsLinkLabel", Do_ClickMeAsync);
        private static async Task Do_ClickMeAsync(SignedCallbackUpdate update)
        {
            // Make localized message; set as edit; send
            await new LocalizedTextMessage("app.clickMeText").Edit(update).AnswerAsync(update);

            // Make localized message
            await new LocalizedTextMessage("app.clickMeText2")
            {
                Menu = new InlineMenu(update.Owner, true, 1)
                {
                    Localize.Inline(new UrlButton("app.JoinUsLabel", $"https://github.com/SKitLs-dev/SKitLs.Bots.Telegram"))
                }
            }.AnswerAsync(update);
        }
    }
}