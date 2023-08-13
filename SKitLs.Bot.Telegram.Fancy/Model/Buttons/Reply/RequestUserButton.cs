using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Reply
{
    /// <summary>
    /// <b>Private chats only.</b> Represents a specific type of button for
    /// <see href="https://core.telegram.org/bots/api#keyboardbuttonrequestuser">requesting user data</see>.
    /// </summary>
    public class RequestUserButton : ReplyButton
    {
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestuser">Telegram API</see>]</b>
        /// <para/>
        /// Signed 32-bit identifier of the request, which will be received back in the UserShared object. Must be unique within the message.
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RequestUserButton"/> class with the specified label, request ID, and optional settings.
        /// </summary>
        /// <param name="label">The label text for the button.</param>
        /// <param name="requestId">The request ID associated with this button.</param>
        /// <param name="singleLine">Indicates whether the button should be displayed in a single line (default is false).</param>
        public RequestUserButton(string label, int requestId, bool singleLine = false) : base(label, singleLine) => RequestId = requestId;

        private bool? IsBot { get; set; }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestuser">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only bots (no users).
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestUserButton OnlyBots()
        {
            IsBot = true;
            return this;
        }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestuser">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only users (no bots).
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestUserButton OnlyUsers()
        {
            IsBot = false;
            return this;
        }
        private bool? IsPremium { get; set; }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestuser">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only premium users.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestUserButton OnlyPremium()
        {
            IsPremium = true;
            return this;
        }
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonrequestuser">Telegram API</see>]</b>
        /// <para/>
        /// Specifies that this button should request only non-premium users.
        /// </summary>
        /// <returns>The current <see cref="RequestUserButton"/> instance.</returns>
        public RequestUserButton OnlyRegular()
        {
            IsPremium = false;
            return this;
        }

        /// <inheritdoc/>
        public override KeyboardButton GetButton() => new(Label)
        {
            RequestUser = new()
            {
                RequestId = RequestId,
                UserIsBot = IsBot,
                UserIsPremium = IsPremium
            },
        };


        /// <inheritdoc/>
        public override object Clone() => new RequestUserButton(Label, RequestId, SingleLine)
        {
            IsBot = IsBot,
            IsPremium = IsPremium,
            ContentBuilder = ContentBuilder,
        };
    }
}