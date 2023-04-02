namespace SKitLs.Bots.Telegram.Core.Model.Builders
{
    /// <summary>
    /// Bot creating enter point. <see cref="BotManager"/> class wizard constructor.
    /// </summary>
    public class BotBuilder
    {
        private readonly BotManager _botManager;

        private BotBuilder(string token) => _botManager = new(token, TgApp.Localizator);
        public static BotBuilder NewBuilder(string token) => new(token);

        //// TODO
        //#region Converter Assets
        ///// <summary>
        ///// Очищает список предустановленных правил конвертаций
        ///// </summary>
        ///// <returns></returns>
        //public BotBuilder ClearConverterRules()
        //{
        //    ArgsConverter.Instance.Clear();
        //    return this;
        //}
        ///// <summary>
        ///// Добавляет новое правило конвертации входящего текстового сообщения в пользовательский класс
        ///// </summary>
        ///// <typeparam name="TResult">Целевой тип конвертации</typeparam>
        ///// <param name="convertRule">Правило конвертации</param>
        //public BotBuilder AddConvertRule<TResult>(ConvertRule<TResult> convertRule)
        //{
        //    ArgsConverter.Instance.AddRule(convertRule);
        //    return this;
        //}
        //#endregion

        public BotBuilder EnablePrivates()
        {
            _botManager.PrivateChatUpdateHandler = new ChatScanner();
            return this;
        }
        /// <summary>
        /// Включает в менеджер бота правила обработки обновлений из личных чатов
        /// </summary>
        /// <param name="builder">Конструктор с собранными правилами</param>
        public BotBuilder EnablePrivatesWith(ChatDesigner builder)
        {
            _botManager.PrivateChatUpdateHandler = builder.Build();
            return this;
        }
        public BotBuilder EnableGroups()
        {
            _botManager.GroupChatUpdateHandler = new ChatScanner();
            return this;
        }
        /// <summary>
        /// Включает в менеджер бота правила обработки обновлений из бесед
        /// </summary>
        /// <param name="builder">Конструктор с собранными правилами</param>
        public BotBuilder EnableGroupWith(ChatDesigner builder)
        {
            _botManager.GroupChatUpdateHandler = builder.Build();
            return this;
        }
        public BotBuilder EnableSupergroups()
        {
            _botManager.SupergroupChatUpdateHandler = new ChatScanner();
            return this;
        }
        /// <summary>
        /// Включает в менеджер бота правила обработки обновлений из супер-бесед
        /// </summary>
        /// <param name="builder">Конструктор с собранными правилами</param>
        public BotBuilder EnableSupergroupsWith(ChatDesigner builder)
        {
            _botManager.SupergroupChatUpdateHandler = builder.Build();
            return this;
        }
        public BotBuilder EnableChannels()
        {
            _botManager.ChannelChatUpdateHandler = new ChatScanner();
            return this;
        }
        /// <summary>
        /// Включает в менеджер бота правила обработки обновлений из каналов
        /// </summary>
        /// <param name="builder">Конструктор с собранными правилами</param>
        public BotBuilder EnableChannelsWith(ChatDesigner builder)
        {
            _botManager.ChannelChatUpdateHandler = builder.Build();
            return this;
        }

        /// <summary>
        /// Возвращает собранного менеджера бота
        /// </summary>
        /// <returns></returns>
        public BotManager Build() => _botManager;
    }
}
