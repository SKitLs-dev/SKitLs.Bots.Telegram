using SKitLs.Bots.Telegram.Core.DeliverySystem;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdateHandlers.Defaults;
using SKitLs.Utils.Localizations.Prototype;

namespace SKitLs.Bots.Telegram.Core.Settings
{
    /// <summary>
    /// Bot's common settings.
    /// </summary>
    public partial class BotSettings
    {
        /// <summary>
        /// Language that is used by <see cref="IDeliveryService"/> by default to send custom system messages to user.
        /// </summary>
        public LangKey BotLanguage { get; set; }

        /// <summary>
        /// Function that determines whether input line is a Bot Command.
        /// Used in <see cref="AnonymMessageTextHandler"/> and in <see cref="SignedMessageTextHandler"/>.
        /// </summary>
        public Func<string, bool> IsCommand { get; set; }

        /// <summary>
        /// Function that gets command data from a command line.
        /// </summary>
        public Func<string, string> GetCommandText { get; set; }

        /// <summary>
        /// Determines whether to make the delivery of messages safe.
        /// Used in <see cref="DefaultDeliveryService"/>.
        /// </summary>
        public bool MakeDeliverySafe { get; set; } = true;

        /// <summary>
        /// Creates a new instance of <see cref="BotSettings"/> used in <see cref="BotManager"/>.
        /// </summary>
        public BotSettings()
        {
            IsCommand = IsCommandM;
            GetCommandText = GetCommandTextM;
        }

        /// <summary>
        /// Default implementation of the <see cref="IsCommand"/> function.
        /// </summary>
        private bool IsCommandM(string command) => command.StartsWith('/');

        /// <summary>
        /// Default implementation of the <see cref="GetCommandText"/> function.
        /// </summary>
        private string GetCommandTextM(string command) => command[1..];
    }
}