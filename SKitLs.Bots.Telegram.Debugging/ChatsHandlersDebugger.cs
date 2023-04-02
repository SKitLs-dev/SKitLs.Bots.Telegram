using SKitLs.Bots.Telegram.Core;
using SKitLs.TGBots.Model.Bot.Interactions;
using SKitLs.TGBots.Model.Loggers;
using SKitLs.TGBots.Model.Managers;
using SKitLs.TGBots.Model.Senders;
using SKitLs.TGBots.Model.StatesSections;

namespace SKitLs.TGBots.Debug
{
    public static class ChatsHandlersDebugger
    {
        private static ILogger Logger { get; set; }
        private static TypedChatUpdateHandler Handler { get; set; }
        private static ICallbackManager Callbacks => Handler.CallbackManager;
        private static ICommandsManager Commands => Handler.MessageHandler.TextMessageUpdateHandler.CommandsManager;
        private static ITextInputManager Input => Handler.MessageHandler.TextMessageUpdateHandler.TextInputManager;

        private static List<UserStateDesc> DeterminedStates { get; set; }

        public static async Task Debug(this ChatBehaviorBuilder builder, bool fullInfo = false, ILogger? logger = null)
            => await builder.Build().Debug(fullInfo, logger);
        internal static async Task Debug(this TypedChatUpdateHandler handler, bool fullInfo = false, ILogger? logger = null)
        {
            Logger = logger ?? new DefaultLogger();
            Handler = handler;

            await CheckUsersInteract(fullInfo);

            DetermineStates(fullInfo);

            Callbacks.GetDefinedCallbacks.CheckCallbacks(fullInfo);
        }

        private static async Task CheckUsersInteract(bool fullInfo)
        {
            int totalTests = 0;
            int success = 0;
            if (fullInfo)
                Logger.System("Проверка входящих пользователей...");

            long testId = 11223344;
            if (Handler.UsersManager == null)
            {
                Logger.Warn("Менеджер пользователей не установлен");

                // Проверка пользователя по умолчанию
                totalTests++;

                IBotUser user = Handler.GetDefaultBotUser(testId);
                if (user == null)
                    Logger.Error("Пользователь по умолчанию вернул NULL. Входящие обновления чата " +
                        "не будут обрабатываться.");
                else if (user.TelegramId != testId)
                    Logger.Error("ID пользователя по умолчанию не соответствовал входным данным. Входящие " +
                        "обновления чата будут обрабатываться неверно.");
                else
                {
                    success++;
                    if (fullInfo)
                        Logger.Success("Пользователь по умолчанию успешно получен");
                }
            }
            else
            {
                Logger.Success("Менеджер пользователей успешно подключён");
                IUsersManager um = Handler.UsersManager;

                // Проверка регистрации
                totalTests++;
                if (await um.IsUserRegistered(0) && await um.IsUserRegistered(long.MaxValue))
                    Logger.Error($"Для тестовых id 0 и {long.MaxValue} проверка на регистрацию успешно пройдена. " +
                        $"Если пользователей с такими id не существует - проверьте корректность метода проверки " +
                        $"регистрации {nameof(IUsersManager.IsUserRegistered)}. Если стоит задача обрабатывать всех " +
                        $"входящих пользователей, воспользуйтесь {nameof(TypedChatUpdateHandler.GetDefaultBotUser)}");
                else
                {
                    success++;
                    if (fullInfo)
                        Logger.Success("Метод проверки регистрации не обнаружил фатальных ошибок.");
                }

                // Проверка создания нового пользователя
                totalTests++;
                IBotUser? user = await um.ProccessNewUser(testId);
                if (user == null)
                    Logger.Error($"Метод нового пользователя {nameof(IUsersManager.ProccessNewUser)} вернул NULL. " +
                        $"Если стоит задача закрыть доступ незарегистрированным пользователям - воспользуйтесь " +
                        $"возвратои нового объекта класса {nameof(DefaultBotUser)}, возвращающего уровень доступа " +
                        $"-1. В противном случае бот не ответит незарегистрированным пользователям.");
                else if (user.TelegramId != testId)
                    Logger.Error("При регистрации нового пользователя вернувшийся новый пользователь не объявлял " +
                        $"параметр {nameof(IBotUser.TelegramId)} как входящий параметр типа long.");
                else
                {
                    success++;
                    if (fullInfo)
                        Logger.Success("Метод регистрации не обнаружил фатальных ошибок.");
                }
            }

            string res = $"Проверка пользователей завершена. Тестов пройдено: {success}/{totalTests}.";
            if (success == totalTests) Logger.Success(res);
            else Logger.Warn(res);
        }
        private static void DetermineStates(bool fullInfo)
        {
            Logger.System("Выявление определённых состояний...");
            List<UserStateDesc> states = new();
            foreach (InteractionStateSection section in Callbacks.GetDefinedCallbacks)
                section.EnabledStates.ForEach(state => states.Add(state));
            foreach (InteractionStateSection section in Commands.GetDefinedCommands)
                section.EnabledStates.ForEach(state => states.Add(state));
            foreach (InteractionStateSection section in Input.GetDefinedInputs)
                section.EnabledStates.ForEach(state => states.Add(state));
            states = states.DistinctBy(x => x.StateId).ToList();
            DeterminedStates = states;
            if (states.Count > 0)
                Logger.Success($"Определено {states.Count} состояний");
            else
                Logger.Error("Не удалось определить состояний. Это может привести к некорректной обработке обновлений");
        }

        private static void CheckCallbacks(this IReadOnlyList<CallbacksStateSection> sections, bool fullInfo)
        {
            int totalTests = 0;
            int success = 0;
            Logger.System("Проверка правил коллбэков...");
            if (fullInfo)
                Logger.System($"Поддерживается {Callbacks.GetDefinedCallbacks.Count} состояний из {DeterminedStates.Count}");

            foreach (var item in sections)
            {
                totalTests++;
                Logger.System($"Проверка секции \"{item.Name}\"");
                int conflicts = CheckCBList(item.AvailableCallbacks, fullInfo);
                if (conflicts > 0) Logger.Error($"Обнаружено конфликтов: {conflicts}");
                else
                {
                    success++;
                    Logger.Success($"Конфликтов не выявлено");
                }
            }
            string mes = $"Проверка секций завершена. Пройдено тестов: {success}/{totalTests}";
            if (totalTests == success) Logger.Success(mes);
            else Logger.Error(mes);

            foreach (var state in DeterminedStates)
            {
                Logger.System($"Проверка состояния: ({state.StateId}) - {state.Name}");
                List<BotCallback> stateCB = new();
                foreach (var section in sections.Where(s => s.EnabledStates.Contains(state.StateId)))
                    section.AvailableCallbacks.ForEach(callback => stateCB.Add(callback));
                
                int conflicts = CheckCBList(stateCB, fullInfo);
                if (conflicts > 0) Logger.Error($"Обнаружено конфликтов: {conflicts}");
                else
                {
                    success++;
                    Logger.Success($"Конфликтов не выявлено");
                }
            }
        }
        private static int CheckCBList(List<BotCallback> callbacks, bool fullInfo)
        {
            int conflicts = 0;
            List<BotCallback> noArgs = callbacks.Where(x => !x.HasArgs).ToList();
            for (int i = 0; i < noArgs.Count; i++)
            {
                BotCallback @base = noArgs[i];
                for (int j = 0; j < noArgs.Count; j++)
                {
                    if (i == j)
                        continue;
                    BotCallback part = noArgs[j];
                    if (@base.Base == part.Base)
                    {
                        conflicts++;
                        if (fullInfo)
                            Logger.Error($"Обнаружено два коллбэка с одинаковым именем {@base.Base}");
                    }
                }
            }
            List<BotCallback> args = callbacks.Where(x => x.HasArgs).ToList();
            for (int i = 0; i < args.Count; i++)
            {
                BotCallback @base = args[i];
                for (int j = 0; j < args.Count; j++)
                {
                    if (i == j)
                        continue;

                    BotCallback part = args[j];
                    if (@base.Base == part.Base)
                    {
                        conflicts++;
                        if (fullInfo)
                        {
                            if (@base.ArgsType.Count != part.ArgsType.Count)
                            {
                                Logger.Error($"Обнаружено два коллбэка с одинаковой базой {@base.Base} и " +
                                    $"разными входными аргументами");
                            }
                            else
                            {
                                bool same = true;
                                for (int k = 0; k < @base.ArgsType.Count; k++)
                                {
                                    if (@base.ArgsType[k] != part.ArgsType[k])
                                    {
                                        same = false;
                                        break;
                                    }
                                }
                                if (same)
                                    Logger.Error($"Обнаружено два коллбэка с одинаковой базой {@base.Base} и " +
                                        $"идентичными входными аргументами");
                                else
                                    Logger.Error($"Обнаружено два коллбэка с одинаковой базой {@base.Base} и " +
                                        $"разными входными аргументами");
                            }
                        }
                    }
                }
            }
            return conflicts;
        }
    }
}
