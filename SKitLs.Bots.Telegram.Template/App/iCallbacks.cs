using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Template.App
{
    internal partial class MainApplicant
    {
        private void EnableCallbacks(IActionManager<SignedCallbackUpdate> entity)
        {
            entity.AddSafely(ClickMeCallback);
        }

        public static DefaultCallback ClickMeCallback { get; } = new("ClickMe", "🔗 Click Me", Do_ClickMeAsync);
        private static async Task Do_ClickMeAsync(SignedCallbackUpdate update)
        {
            // Get localized text
            var text = update.Owner.ResolveBotString("app.clickMeText");
            // Edit message-sender text
            await update.Owner.DeliveryService.AnswerSenderAsync(await EditWrapper.FromBuildable(new OutputMessageText(text), update), update);

            // Get localized text
            var inviteText = update.Owner.ResolveBotString("app.clickMeText2");
            var inviteMessage = new OutputMessageText(inviteText);
            
            var inviteMenu = new InlineMenu(update.Owner);
            inviteMenu.Add(new UrlButton("Join us!", $"https://github.com/SKitLs-dev/SKitLs.Bots.Telegram"));
            inviteMessage.Menu = inviteMenu;
            
            await update.Owner.DeliveryService.AnswerSenderAsync(await inviteMessage.BuildContentAsync(update), update);
        }
    }
}
