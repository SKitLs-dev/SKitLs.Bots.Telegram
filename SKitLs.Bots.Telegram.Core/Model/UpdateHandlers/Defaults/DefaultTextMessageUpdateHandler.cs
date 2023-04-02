//using SKitLs.Bots.Telegram.Core.CastedUpdates.MessageUpdates;
//using SKitLs.Bots.Telegram.Core.UpdateHandlers;
//using SKitLs.TGBots.Model.Managers;

//namespace SKitLs.TGBots.Model.Bot.UpdateHandlers.MessgeUpdates
//{
//    internal class DefaultTextMessageUpdateHandler : ITextMessageUpdateHandler
//    {
//        public Func<string, bool> IsCommand { get; set; }
//        public ICommandsManager CommandsManager { get; set; }
//        public ITextInputManager TextInputManager { get; set; }

//        public DefaultTextMessageUpdateHandler()
//        {
//            CommandsManager = new DefaultCommandsManager();
//            TextInputManager = new DefaultTextInputManager();
//            IsCommand = (string input) => input.StartsWith('/');
//        }

//        public async Task HandleUpdateAsync(SignedMessageTextUpdate update)
//        {
//            if (IsCommand(update.Text))
//            {
//                await CommandsManager.HandleUpdateAsync(update);
//            }
//            else
//            {
//                await TextInputManager.HandleUpdateAsync(update);
//            }
//        }
//    }
//}
