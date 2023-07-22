using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace Tester
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var privateMessages = new DefaultSignedMessageUpdateHandler();
            var privateTexts = new DefaultSignedMessageTextUpdateHandler
            {
                CommandsManager = new DefaultActionManager<SignedMessageTextUpdate>()
            };
            privateTexts.CommandsManager.AddSafely(StartCommand);
            privateMessages.TextMessageUpdateHandler = privateTexts;

            ChatDesigner privates = ChatDesigner.NewDesigner()
               .UseMessageHandler(privateMessages);

            await BotBuilder.NewBuilder("YOUR_TOKEN")
               .EnablePrivates(privates)
               .Build()
               .Listen();
        }

        private static DefaultCommand StartCommand => new("start", Do_StartAsync);
        private static async Task Do_StartAsync(SignedMessageTextUpdate update)
        {
            await update.Owner.DeliveryService.ReplyToSender("Hello, world!", update);
        }
    }
}