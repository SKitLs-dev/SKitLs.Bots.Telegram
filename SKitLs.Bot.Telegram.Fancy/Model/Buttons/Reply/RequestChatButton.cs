using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Reply
{
    /// <summary>
    /// <b>Private chats only.</b> Represents a specific type of button for
    /// <see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">requesting user data</see>.
    /// </summary>
    public class RequestChatButton : ReplyButton
    {
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Signed 32-bit identifier of the request, which will be received back in the UserShared object. Must be unique within the message.
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Pass <see langword="true"/> to request a channel chat, pass <see langword="false"/> to request a group or a supergroup chat.
        /// </summary>
        public bool ChatIsChannel { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RequestUserButton"/> class with the specified label, request ID, and optional settings.
        /// </summary>
        /// <param name="label">The label text for the button.</param>
        /// <param name="requestId">The request ID associated with this button.</param>
        /// <param name="isChannel">Pass <see langword="true"/> to request a channel chat.</param>
        /// <param name="singleLine">Indicates whether the button should be displayed in a single line.</param>
        public RequestChatButton(string label, int requestId, bool isChannel = false, bool singleLine = false) : base(label, singleLine)
        {
            RequestId = requestId;
            ChatIsChannel = isChannel;
        }

        private bool? ChatIsForum { get; set; }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only forum chats.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestChatButton OnlyForums()
        {
            ChatIsForum = true;
            return this;
        }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only group oe supergroup chats.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestChatButton NotForums()
        {
            ChatIsForum = false;
            return this;
        }

        private bool? ChatHasUsername { get; set; }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only supergroups or channels with a username.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestChatButton OnlyNamed()
        {
            ChatHasUsername = true;
            return this;
        }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only chats without a username.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestChatButton NotNamed()
        {
            ChatHasUsername = false;
            return this;
        }
        // chat_is_created 	Boolean
        private bool? ChatIsCreated { get; set; }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only chats owned by the user.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestChatButton OnlyOwned()
        {
            ChatIsCreated = true;
            return this;
        }
        ///// <summary>
        ///// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        ///// <para/>
        ///// Specifies that this button should request only chats without a username.
        ///// </summary>
        ///// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        //public RequestChatButton NotOwned()
        //{
        //    ChatIsCreated = false;
        //    return this;
        //}

        private bool BotIsMember { get; set; }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only chats with the bot as a member.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestChatButton OnlyMember()
        {
            BotIsMember = true;
            return this;
        }

        private ChatAdministratorRights? UserRights { get; set; }
        private ChatAdministratorRights? BotRights { get; set; }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestchat">Telegram API</see>]</b>
        /// <para/>
        /// Lists the required administrator rights of the user in the chat.
        /// The <paramref name="userRights"/> must be a superset of <paramref name="botRights"/>.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestChatButton AdministrativeRules(ChatAdministratorRights userRights, ChatAdministratorRights botRights)
        {
            UserRights = userRights;
            BotRights = botRights;
            return this;
        }

        /// <inheritdoc/>
        public override KeyboardButton GetButton() => new(Label)
        {
            RequestChat = new()
            {
                RequestId = RequestId,
                BotIsMember = BotIsMember,
                ChatHasUsername = ChatHasUsername,
                ChatIsCreated = ChatIsCreated,
                ChatIsChannel = ChatIsChannel,
                ChatIsForum = ChatIsForum,
                UserAdministratorRights = UserRights,
                BotAdministratorRights = BotRights,
            }
        };

        /// <inheritdoc/>
        public override object Clone() => new RequestChatButton(Label, RequestId, ChatIsChannel, SingleLine)
        {
            BotIsMember = BotIsMember,
            ChatHasUsername = ChatHasUsername,
            ChatIsCreated = ChatIsCreated,
            ChatIsForum = ChatIsForum,
            ContentBuilder = ContentBuilder,
            BotRights = BotRights,
            UserRights = UserRights,
        };
    }
}