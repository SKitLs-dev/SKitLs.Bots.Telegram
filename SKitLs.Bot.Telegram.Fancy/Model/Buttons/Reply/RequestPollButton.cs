using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Buttons.Reply
{
    /// <summary>
    /// <b>Private chats only.</b> Represents a button that requests the user to create and send a
    /// <see href="https://core.telegram.org/bots/api#keyboardbuttonpolltype">poll</see> when pressed.
    /// </summary>
    public class RequestPollButton : ReplyButton
    {
        /// <summary>
        /// <b>[<see href="https://core.telegram.org/bots/api#keyboardbuttonpolltype">Telegram API</see>]</b>
        /// <para/>
        /// Represents the type of the poll which is allowed to be created when the button is pressed.
        /// When <see langword="null"/> the user will be allowed to create a poll of any type.
        /// </summary>
        public PollType? Type { get; set; }

        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of the <see cref="RequestPollButton"/> class with the specified label and optional settings.
        /// </summary>
        public RequestPollButton(string label, bool singleLine = false) : base(label, singleLine) { }

        /// <summary>
        /// Specifies that the user will be allowed to create only polls in the quiz mode.
        /// </summary>
        /// <returns>The current instance of <see cref="RequestPollButton"/>.</returns>
        public RequestPollButton Quiz()
        {
            Type = PollType.Quiz;
            return this;
        }
        /// <summary>
        /// Specifies that the user will be allowed to create only polls in the regular mode.
        /// </summary>
        /// <returns>The current instance of <see cref="RequestPollButton"/>.</returns>
        public RequestPollButton Regular()
        {
            Type = PollType.Regular;
            return this;
        }

        /// <inheritdoc/>
        public override KeyboardButton GetButton() => new(Label)
        {
            RequestPoll = new()
            {
                Type = Type is not null ? Enum.GetName(typeof(PollType), Type) : null,
            }
        };
    }
}