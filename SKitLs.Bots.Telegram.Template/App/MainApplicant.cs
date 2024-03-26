using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Model;
using Telegram.Bot;

namespace SKitLs.Bots.Telegram.Template.App
{
    internal partial class MainApplicant :
        IApplicant<IActionManager<SignedMessageTextUpdate>>,
        IApplicant<IActionManager<SignedCallbackUpdate>>
    {
        public void ApplyTo(IActionManager<SignedCallbackUpdate> entity)
        {
            EnableCallbacks(entity);
        }

        public void ApplyTo(IActionManager<SignedMessageTextUpdate> entity)
        {
            EnableCommands(entity);
        }

        internal static async Task OpenMainMenuAsync(ISignedUpdate update)
        {
            var mm = update.Owner.ResolveService<IMenuService>();
            if (update is SignedCallbackUpdate callback)
                await update.Owner.Bot.EditMessageReplyMarkupAsync(update.ChatId, callback.TriggerMessageId, null);
            var page = mm.GetDefined(MainPageId);
            await mm.PushPageAsync(page, update, true);
        }
    }
}