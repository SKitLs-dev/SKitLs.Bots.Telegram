using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;

namespace Tester
{
    internal class Program
    {
        // Использовать БД пользователей
        // bot.UseUsersDataList(...);
        // ~~~
        // bot.UseUsersManager();

        // Использовать Систему Прав
        // bot.UseAuthSystem(...);

        // Регистрация сущностей
        //

        // Включить мозги программы
        // bot.UseBotInteraction(...);

        // bot.Listen();
        // 

        static async Task Main(string[] args)
        {
            ChatDesigner privates = ChatDesigner.NewChatDesigner();
            privates.UseMessageHandler(new UHBInformer("Message", true, true));

            // Customers Panel
            // Couches Panel

            BotBuilder builder = new("1884746031:AAF_JtOS882Uz33IXlNtpyyQUoLTGSkvP9I");
            builder.EnablePrivatesWith(privates);
            await builder.Build().Listen();
        }
    }
}