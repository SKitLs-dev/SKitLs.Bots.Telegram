using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem;
using SKitLs.Utils.Localizations.Prototype;

namespace SKitLs.Bots.Telegram.Core.resources.Settings
{
    /// <summary>
    /// Bot's common settings.
    /// </summary>
    public partial class BotSettings
    {
        /// <summary>
        /// /// Language that is used by <see cref="IDelieveryService"/> by default to send custom system messages to user.
        /// </summary>
        public LangKey BotLanguage { get; set; }
        
        /// <summary>
        /// Function that determines whether input line is a Bot Command.
        /// </summary>
        public Func<string, bool> IsCommand { get; set; }
        /// <summary>
        /// Function that gets command data from a command line.
        /// </summary>
        public Func<string, string> GetCommandText { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="BotSettings"/> that used in <see cref="BotManager"/>.
        /// </summary>
        public BotSettings()
        {
            IsCommand = IsCommandM;
            GetCommandText = GetCommandTextM;
        }

        /// <summary>
        /// Default realization of the <see cref="IsCommand"/>.
        /// </summary>
        private bool IsCommandM(string command) => command.StartsWith('/');
        /// <summary>
        /// Default realization of the <see cref="GetCommandText"/>.
        /// </summary>
        private string GetCommandTextM(string command) => command[1..];
    }
}