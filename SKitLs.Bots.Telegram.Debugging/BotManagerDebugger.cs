using SKitLs.Bots.Telegram.Core;
using SKitLs.TGBots.Model.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SKitLs.TGBots.Debug
{
    public static class BotManagerDebugger
    {
        private static ILogger Logger { get; set; }

        static BotManagerDebugger() => Logger = new DefaultLogger();

        public static async Task Debug(this BotManager manager, bool fullInfo = false)
        {
            if (manager.Logger == null) Logger.Error("Менеджер не объявляет Логгер. Для отладки будет " +
                "использован Логгер по умолчанию. Во избежании ошибок передайте в билдер Логгер, отличный от NULL");
            Logger = manager.Logger ?? new DefaultLogger();

            // Проверка связи с ботом
            bool @continue = await CheckBot(manager, fullInfo);
            if (!@continue) return;

            // Информация об обработчиках чатов
            CheckChatsHandlers(manager);

            // Проверка обработчиков чатов
            await ValidChats(manager, fullInfo);
        }

        private static async Task<bool> CheckBot(BotManager manager, bool fullInfo)
        {
            bool @continue = true;
            Logger.System("<-- Начата проверка менеджера -->");
            if (fullInfo)
                Logger.System("Проверка бота...");
            if (!manager.IsTokenDefined)
            {
                Logger.Error("Токен бота не определён");
                Logger.Warn("Продолжить? (y/n)", false);
                string? ok = Console.ReadLine();
                if (!string.IsNullOrEmpty(ok) && ok.ToLower() == "y") @continue = true;
            }
            else
            {
                if (fullInfo)
                {
                    Logger.Success("Токен бота определён");
                    Logger.System("Проверка связи с ботом...");
                }
                CancellationTokenSource cts = new();
                try
                {
                    User me = await manager.Bot.GetMeAsync(cts.Token);
                    Logger.Success($"Бот успешно ответил. Имя бота: @{me.Username}");
                }
                catch (Exception ex)
                {
                    cts.Cancel();
                    Logger.Warn("Ошибка связи с ботом.");
                    Logger.Log(ex);
                    Logger.Warn("Продолжить? (y/n)", false);
                    string? ok = Console.ReadLine();
                    @continue = (!string.IsNullOrEmpty(ok) && ok.ToLower() == "y");
                }
            }
            Logger.Line();
            return @continue;
        }
        private static void CheckChatsHandlers(BotManager manager)
        {
            Logger.System("Состояние чатов:");
            if (manager.PrivateChatUpdateHandler != null)
                Logger.Success("Приватные чаты: включены");
            else
                Logger.Warn("Приватные чаты: игнорируются");

            if (manager.ChannelChatUpdateHandler != null)
                Logger.Success("Каналы: включены");
            else
                Logger.Warn("Каналы: игнорируются");

            if (manager.GroupChatUpdateHandler != null)
                Logger.Success("Группы: включены");
            else
                Logger.Warn("Группы: игнорируются");

            if (manager.SupergroupChatUpdateHandler != null)
                Logger.Success("Супергруппы: включены");
            else
                Logger.Warn("Супергруппы: игнорируются");

            if (manager.GroupChatUpdateHandler == manager.SupergroupChatUpdateHandler && manager.GroupChatUpdateHandler != null)
                Logger.Warn("Группы и супергруппы проверяются по одинаковым правилам");

            Logger.Line();
        }
        private static async Task ValidChats(BotManager manager, bool fullInfo)
        {

            if (manager.PrivateChatUpdateHandler != null)
            {
                Logger.System("Проверка приватных чатов...");
                await manager.PrivateChatUpdateHandler.Debug(fullInfo);
                Logger.Line();
            }
            else
            {
                if (fullInfo)
                    Logger.Warn("Приватные чаты пропущены...");
            }

            if (manager.ChannelChatUpdateHandler != null)
            {
                Logger.System("Проверка каналов...");
                await manager.ChannelChatUpdateHandler.Debug(fullInfo);
                Logger.Line();
            }
            else
            {
                if (fullInfo)
                    Logger.Warn("Каналы пропущены...");
            }

            if (manager.GroupChatUpdateHandler != null)
            {
                Logger.System("Проверка групп...");
                await manager.GroupChatUpdateHandler.Debug(fullInfo);
                Logger.Line();
            }
            else
            {
                if (fullInfo)
                    Logger.Warn("Группы пропущены...");
            }

            if (manager.SupergroupChatUpdateHandler != null)
            {
                Logger.System("Проверка супергрупп...");
                await manager.SupergroupChatUpdateHandler.Debug(fullInfo);
                Logger.Line();
            }
            else
            {
                if (fullInfo)
                    Logger.Warn("Супергруппы пропущены...");
            }
        }

    }
}
