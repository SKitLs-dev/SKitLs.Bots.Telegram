using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Reply
{
    /// <summary>
    /// Represents a specific menu designed to remove a custom reply keyboard (<see cref="ReplyMenu"/>)
    /// by using the <see cref="ReplyKeyboardRemove"/> feature.
    /// </summary>
    public class ReplyCleaner : IMessageMenu
    {
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#replykeyboardmarkup">Telegram API</see>]</b>
        /// <para/>
        /// Use this parameter if you want to show the keyboard to specific users only.
        /// Targets:
        /// <list type="number">
        /// <item>Users that are @mentioned in the text of the Message object</item>
        /// <item>If the bot's message is a reply (has <see href="https://core.telegram.org/bots/api#sendmessage">reply_to_message_id</see>),
        /// sender of the original message</item>
        /// </list>
        /// <para/>
        /// Example: A user requests to change the bot's language, bot replies to the request with a keyboard to select the new language.
        /// Other users in the group don't see the keyboard.
        /// </summary>
        public bool Selective { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ReplyCleaner"/> class with a specified data.
        /// </summary>
        /// <param name="selective">Determines whether the keyboard should be shown to specific users only.</param>
        public ReplyCleaner(bool selective = false) => Selective = selective;

        /// <inheritdoc/>
        public IReplyMarkup GetMarkup() => new ReplyKeyboardRemove()
        {
            Selective = Selective
        };

        /// <inheritdoc/>
        public object Clone() => new ReplyCleaner(Selective);
    }
}