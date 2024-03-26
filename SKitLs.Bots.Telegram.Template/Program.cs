using SKitLs.Bots.Telegram.AdvancedMessages.AdvancedDelivery;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdateHandlers.Defaults;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.Template.App;
using SKitLs.Bots.Telegram.Template.Services.Model;
using SKitLs.Bots.Telegram.Template.Services.Prototype;
using SKitLs.Utils.Localizations.Prototype;

namespace SKitLs.Bots.Telegram.Template
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var applicant = new MainApplicant();
                var usersManager = new UsersManager();
                var menuManager = applicant.GetMenuManager();

                // Setup Private Messages
                var privateMessages = new SignedMessageBaseHandler();

                // Setup Commands
                var privateCommands = new LinearActionManager<SignedMessageTextUpdate>("commands.private");
                applicant.ApplyTo(privateCommands);

                var privateTexts = new SignedMessageTextHandler
                {
                    CommandsManager = privateCommands,
                };
                privateMessages.TextMessageUpdateHandler = privateTexts;

                // Setup private callbacks
                var privateCallbacks = new LinearActionManager<SignedCallbackUpdate>("callbacks.private");
                // Apply - Main Logic
                applicant.ApplyTo(privateCallbacks);
                // Apply - Menu Manager
                menuManager.ApplyTo(privateCallbacks);

                var privateCallbacksHandler = new CallbackHandler()
                {
                    CallbackManager = privateCallbacks,
                };

                // Rest
                ChatDesigner privates = ChatDesigner.NewDesigner()
                    .UseUsersManager(usersManager)
                    .UseMessageHandler(privateMessages)
                    .UseCallbackHandler(privateCallbacksHandler);

                // your_api_key
                var bot = BotBuilder.NewBuilder("1884746031:AAG2De0kmRcogBNO_NyWMU-9E3wxE2MUBrc")
                    .CustomDelivery(new AdvancedDeliveryService())
                    .EnablePrivates(privates)
                    // Optional: use same chatter / create new one
                    //.EnableGroups(privates)
                    //.EnableSupergroups(groups)
                    
                    .AddService<IArgsSerializeService>(new ArgsSerializeService())
                    .AddService<IMenuService>(menuManager)
                    .AddService<ICBDemoService>(new CBDemoService_v2("Answer"))
                    //.AddService<ICBDemoService>(new CBDemoService_v1())
                    .Build();

                BotBuilder.DebugSettings.LogExceptionTrace = true;
                bot.Settings.BotLanguage = LangKey.RU;
                await bot.Listen();
            }
            catch (Exception e)
            {
                BotBuilder.DebugSettings.LocalLogger.Log(e);
                return;
            }
        }
    }
}