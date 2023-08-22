using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Payments;

namespace SKitLs.Bots.Telegram.Core.Model.Services
{
    public class PreCheckoutService : IPreCheckoutService
    {
        private BotManager? _owner;
        /// <inheritdoc/>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <inheritdoc/>
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Provides quick access to the bot client by the entity's owner.
        /// </summary>
        private ITelegramBotClient Bot => Owner.Bot;

        public async Task HandlePreCheckoutAsync(PreCheckoutQuery preCheckout)
        {
            await Bot.AnswerPreCheckoutQueryAsync(preCheckout.Id);
        }
    }
}