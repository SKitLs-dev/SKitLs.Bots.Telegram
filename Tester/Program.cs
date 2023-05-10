using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

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
            ChatDesigner privates = ChatDesigner.NewDesigner();
            privates.UseMessageHandler(new UHBInformer<SignedMessageUpdate>("Message", true, true));

            //// Customers Panel
            //// Couches Panel

            BotBuilder builder = BotBuilder.NewBuilder("1884746031:AAF_JtOS882Uz33IXlNtpyyQUoLTGSkvP9I");
            builder.EnablePrivates(privates);
            //await builder.Build().Listen();
            builder.Build();
        }
    }
}