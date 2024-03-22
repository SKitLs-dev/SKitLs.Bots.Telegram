using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// Provides helper methods for working with Telegram updates.
    /// </summary>
    public static class TelegramHelper
    {
        /// <summary>
        /// Extracts the <see cref="ChatType"/> from a raw server update.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <param name="requester">The object requesting the method. Use <c>this</c> statement.</param>
        /// <returns>The chat type of the update.</returns>
        /// <exception cref="BotManagerException">Thrown when the chat type cannot be determined.</exception>
        public static ChatType GetChatType(Update update, object requester) => TryGetChatType(update)
            ?? throw new BotManagerException("bm.ChatNotHandled", requester);

        /// <summary>
        /// Tries to extract the <see cref="ChatType"/> from a raw server update.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <returns>The chat type of the update, or <see langword="null"/> if it cannot be determined.</returns>
        public static ChatType? TryGetChatType(Update update)
        {
            // Polls ??
            // Shipping query ??
            if (update.CallbackQuery is not null && update.CallbackQuery.Message is not null)
                return update.CallbackQuery.Message.Chat?.Type;
            else if (update.ChannelPost is not null)
                return update.ChannelPost.Chat?.Type;
            else if (update.ChatJoinRequest is not null)
                return update.ChatJoinRequest.Chat?.Type;
            else if (update.ChatMember is not null)
                return update.ChatMember.Chat?.Type;
            else if (update.EditedChannelPost is not null)
                return update.EditedChannelPost.Chat?.Type;
            else if (update.EditedMessage is not null)
                return update.EditedMessage.Chat?.Type;
            else if (update.InlineQuery is not null)
                return update.InlineQuery.ChatType;
            else if (update.Message is not null)
                return update.Message.Chat?.Type;
            else if (update.MyChatMember is not null)
                return update.MyChatMember.Chat?.Type;

            else if (update.ChosenInlineResult is not null)
                return ChatType.Sender;
            else return null;
        }

        /// <summary>
        /// Extracts the sender's ID from a raw server update or throws a <see cref="BotManagerException"/> if unsuccessful.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <param name="requester">The object requesting the method. Use <c>this</c> statement.</param>
        /// <returns>The sender's ID.</returns>
        /// <exception cref="BotManagerException">Thrown when the sender's ID cannot be extracted.</exception>
        public static long GetSenderId(Update update, object requester) => TryGetSenderId(update)
            ?? throw new BotManagerException("cs.UserIdExtractError", requester, update.Id.ToString());

        /// <summary>
        /// Tries to extract the sender's ID from a raw server update.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <returns>The sender's ID if available; otherwise, <see langword="null"/>.</returns>
        public static long? TryGetSenderId(Update update)
        {
            if (update.CallbackQuery != null && update.CallbackQuery.Message != null)
                return update.CallbackQuery.From?.Id;
            else if (update.ChannelPost != null)
                return update.ChannelPost.From?.Id;
            else if (update.ChatJoinRequest != null)
                return update.ChatJoinRequest.From?.Id;
            else if (update.ChatMember != null)
                return update.ChatMember.From?.Id;
            else if (update.EditedChannelPost != null)
                return update.EditedChannelPost.From?.Id;
            else if (update.EditedMessage != null)
                return update.EditedMessage.From?.Id;
            else if (update.Message != null)
                return update.Message.From?.Id;
            else if (update.MyChatMember != null)
                return update.MyChatMember.From?.Id;
            else return null;
        }

        /// <summary>
        /// Extracts an instance of the sender (<see cref="User"/>) from a raw server update or throws a <see cref="BotManagerException"/> if unsuccessful.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <param name="requester">The object that has requested the extraction.</param>
        /// <returns>The instance of the sender.</returns>
        /// <exception cref="BotManagerException">Thrown when the sender instance cannot be extracted.</exception>
        public static User GetSender(Update update, object requester) => TryGetSender(update)
            ?? throw new BotManagerException("cs.UserExtractError", requester, update.Id.ToString());

        /// <summary>
        /// Tries to extract an instance of the sender (<see cref="User"/>) from a raw server update.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <returns>The instance of the sender if available; otherwise, <see langword="null"/>.</returns>
        public static User? TryGetSender(Update update)
        {
            if (update.CallbackQuery != null && update.CallbackQuery.Message != null)
                return update.CallbackQuery.From;
            else if (update.ChannelPost != null)
                return update.ChannelPost.From;
            else if (update.ChatJoinRequest != null)
                return update.ChatJoinRequest.From;
            else if (update.ChatMember != null)
                return update.ChatMember.From;
            else if (update.EditedChannelPost != null)
                return update.EditedChannelPost.From;
            else if (update.EditedMessage != null)
                return update.EditedMessage.From;
            else if (update.Message != null)
                return update.Message.From;
            else if (update.MyChatMember != null)
                return update.MyChatMember.From;
            else return null;
        }

        /// <summary>
        /// Extracts the chat ID from a raw server update.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <param name="requester">The object requesting the method. Use <c>this</c> statement.</param>
        /// <returns>The chat ID of the update.</returns>
        /// <exception cref="BotManagerException">Thrown when the chat ID cannot be determined.</exception>
        public static long GetChatId(Update update, object requester) => TryGetChatId(update)
            ?? throw new BotManagerException("bm.ChatIdNotHandled", requester);

        /// <summary>
        /// Tries to extract the chat ID from a raw server update.
        /// </summary>
        /// <param name="update">The original server update.</param>
        /// <returns>The chat ID of the update, or <see langword="null"/> if it cannot be determined.</returns>
        public static long? TryGetChatId(Update update)
        {
            // Polls ??
            // Shipping query ??
            // Inline query ??
            // ChosenInlineResult ?? 
            if (update.CallbackQuery != null && update.CallbackQuery.Message != null)
                return update.CallbackQuery.Message.Chat?.Id;
            else if (update.ChannelPost != null)
                return update.ChannelPost.Chat?.Id;
            else if (update.ChatJoinRequest != null)
                return update.ChatJoinRequest.Chat?.Id;
            else if (update.ChatMember != null)
                return update.ChatMember.Chat?.Id;
            else if (update.EditedChannelPost != null)
                return update.EditedChannelPost.Chat?.Id;
            else if (update.EditedMessage != null)
                return update.EditedMessage.Chat?.Id;
            else if (update.Message != null)
                return update.Message.Chat?.Id;
            else if (update.MyChatMember != null)
                return update.MyChatMember.Chat?.Id;
            else return null;
        }

        /// <summary>
        /// Returns a list of update types that can be received from <see cref="ChatType.Private"/>.
        /// </summary>
        [Obsolete("Hardcoded. May be incorrect.")]
        public static List<UpdateType> PrivateUpdates { get; set; } = new()
        {
            UpdateType.Message,
            UpdateType.EditedMessage,
            UpdateType.CallbackQuery,
            UpdateType.InlineQuery,
            UpdateType.PreCheckoutQuery,
            UpdateType.ShippingQuery,
        };

        /// <summary>
        /// Returns a list of update types that can be received from <see cref="ChatType.Group"/>.
        /// </summary>
        [Obsolete("Hardcoded. May be incorrect.")]
        public static List<UpdateType> GroupUpdates { get; set; } = new()
        {
            UpdateType.Message,
            UpdateType.EditedMessage,
            UpdateType.CallbackQuery,
            UpdateType.InlineQuery,
            UpdateType.PreCheckoutQuery,
            UpdateType.ShippingQuery,

            UpdateType.ChatJoinRequest,
            UpdateType.ChatMember,
            UpdateType.MyChatMember,

            UpdateType.Poll,
            UpdateType.PollAnswer,
        };
        
        /// <summary>
        /// Returns a list of update types that can be received from <see cref="ChatType.Supergroup"/>.
        /// </summary>
        [Obsolete("Hardcoded. May be incorrect.")]
        public static List<UpdateType> SupergroupUpdates { get; set; } = new()
        {
            UpdateType.Message,
            UpdateType.EditedMessage,
            UpdateType.CallbackQuery,
            UpdateType.InlineQuery,
            UpdateType.PreCheckoutQuery,
            UpdateType.ShippingQuery,

            UpdateType.ChatJoinRequest,
            UpdateType.ChatMember,
            UpdateType.MyChatMember,

            UpdateType.Poll,
            UpdateType.PollAnswer,
        };

        /// <summary>
        /// Returns a list of update types that can be received from <see cref="ChatType.Channel"/>.
        /// </summary>
        [Obsolete("Hardcoded. May be incorrect.")]
        public static List<UpdateType> ChannelUpdates { get; set; } = new()
        {
            UpdateType.ChannelPost,
            UpdateType.EditedChannelPost,
            UpdateType.CallbackQuery,
            UpdateType.InlineQuery,

            UpdateType.ChatJoinRequest,
            UpdateType.ChatMember,
            UpdateType.MyChatMember,

            UpdateType.Poll,
            UpdateType.PollAnswer,
        };
    }
}
