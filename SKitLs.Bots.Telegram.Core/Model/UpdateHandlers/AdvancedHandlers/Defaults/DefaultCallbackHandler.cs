using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.Management.Defaults;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers.AdvancedHandlers.Defaults
{
    public class DefaultCallbackHandler : IUpdateHandlerBase<SignedCallbackUpdate>
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        public IActionManager<SignedCallbackUpdate> CallbackManager { get; set; }

        public DefaultCallbackHandler()
        {
            CallbackManager = new DefaultActionManager<SignedCallbackUpdate>();
        }

        public Task HandleUpdateAsync(CastedUpdate update, IBotUser? sender)
            => HandleUpdateAsync(BuildUpdate(update, sender));
        public SignedCallbackUpdate BuildUpdate(CastedUpdate update, IBotUser? sender)
        {
            if (sender is null)
                throw new NullSenderException();
            return new(update, sender);
        }
        public async Task HandleUpdateAsync(SignedCallbackUpdate update)
        {
            await CallbackManager.ManageUpdateAsync(update);
        }
    }
}