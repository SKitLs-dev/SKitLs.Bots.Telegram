using SKitLs.Bots.Telegram.AdvancedMessages.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Messages.Text
{
    /// <summary>
    /// Represents a message that retrieves localized text based on localized keys for various languages.
    /// </summary>
    public class LocalizedTextMessage : Localized<OutputMessageText, TelegramTextMessage, ITelegramMessage>, IBuildableMessage
    {
        /// <summary>
        /// Represents the message's menu, if applicable.
        /// </summary>
        public virtual IBuildableContent<IMessageMenu>? Menu
        {
            get => Value.Menu;
            set => Value.Menu = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedTextMessage"/> class with the specified localized string key.
        /// </summary>
        /// <param name="textLocalKey">The key used to retrieve the localized text.</param>
        /// <param name="format">Optional format arguments used for formatting the localized text.</param>
        public LocalizedTextMessage(string textLocalKey, params string?[] format) : base(new OutputMessageText(textLocalKey), nameof(TelegramTextMessage.Text), format)
        { }
    }
}